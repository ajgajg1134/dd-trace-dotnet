#pragma once

#include <chrono>
#include <memory>
#include <regex>
#include <sstream>
#include <type_traits>

#include <spdlog/sinks/null_sink.h>
#include <spdlog/sinks/rotating_file_sink.h>
#include <spdlog/spdlog.h>

#include "dd_filesystem.hpp"
#include "pal.h"
#include "string.h"

namespace datadog::shared
{

/// <summary>
/// Logger class is created only by LogManager class.
/// </summary>
class Logger
{
public:

    template <typename... Args>
    void Debug(const Args&... args);

    template <typename... Args>
    void Info(const Args&... args);

    template <typename... Args>
    void Warn(const Args&... args);

    template <typename... Args>
    void Error(const Args&... args);

    template <typename... Args>
    void Critical(const Args&... args);

    inline void Flush();

    inline void EnableDebug(bool enable);
    inline bool IsDebugEnabled() const;


private:

    friend class LogManager;

    Logger(std::shared_ptr<spdlog::logger> const& logger) : _internalLogger{logger}, m_debug_logging_enabled{false}
    {
    }

    ~Logger()
    {
        _internalLogger->flush();
        spdlog::shutdown(); // <--- Not sure we still should do that since in the same process we could have Tracer Logger
    }

    template <class LoggerPolicy>
    static Logger Create();

    static inline std::string SanitizeProcessName(std::string const& processName);
    static inline std::string BuildLogFileSuffix();

    template <class LoggerPolicy>
    static std::string GetLogPath(const std::string& file_name_suffix);

    template <class LoggerPolicy>
    static std::shared_ptr<spdlog::logger> CreateInternalLogger();

    std::shared_ptr<spdlog::logger> _internalLogger;
    bool m_debug_logging_enabled;
};

template <class LoggerPolicy>
inline Logger Logger::Create()
{
    return {Logger::CreateInternalLogger<LoggerPolicy>()};
}

inline std::string Logger::SanitizeProcessName(std::string const& processName)
{
    const auto process_name_without_extension = processName.substr(0, processName.find_last_of("."));
    const std::regex dash_or_space_or_tab("-|\\s|\\t");
    return std::regex_replace(process_name_without_extension, dash_or_space_or_tab, "_");
}

inline std::string Logger::BuildLogFileSuffix()
{
    std::ostringstream oss;

    auto processName = Logger::SanitizeProcessName(::shared::ToString(::shared::GetCurrentProcessName()));
    oss << "-" << processName << "-" << ::shared::GetPID();
    return oss.str();
}

template <class LoggerPolicy>
std::shared_ptr<spdlog::logger> Logger::CreateInternalLogger()
{
    spdlog::set_error_handler([](const std::string& msg) {
        // By writing into the stderr was changing the behavior in a CI scenario.
        // There's not a good way to report errors when trying to create the log file.
        // But we never should be changing the normal behavior of an app.
        // std::cerr << "LoggerImpl Handler: " << msg << std::endl;
    });

    spdlog::flush_every(std::chrono::seconds(3));

    static auto file_name_suffix = Logger::BuildLogFileSuffix();

    std::shared_ptr<spdlog::logger> logger;

    try
    {
        logger =
            spdlog::rotating_logger_mt(LoggerPolicy::file_name, Logger::GetLogPath<LoggerPolicy>(file_name_suffix), 1048576 * 5, 10);
    }
    catch (...)
    {
        // By writing into the stderr was changing the behavior in a CI scenario.
        // There's not a good way to report errors when trying to create the log file.
        // But we never should be changing the normal behavior of an app.
        // std::cerr << "LoggerImpl Handler: Error creating native log file." << std::endl;
        logger = spdlog::null_logger_mt("LoggerImpl");
    }

    logger->set_level(spdlog::level::debug);

    logger->set_pattern(LoggerPolicy::pattern);

    logger->flush_on(spdlog::level::info);

    return logger;
}

template <class TLoggerPolicy>
std::string Logger::GetLogPath(const std::string& file_name_suffix)
{
    const auto log_path = fs::path(::shared::GetDatadogLogFilePath<TLoggerPolicy>(file_name_suffix));

    if (log_path.has_parent_path())
    {
        const auto parent_path = log_path.parent_path();

        if (!fs::exists(parent_path))
        {
            fs::create_directories(parent_path);
        }
    }

    return log_path.string();
}


// On Debian buster, we only have libstdc++ 8 which does not have a definition for the std::same_as concept
// and std::remove_cvref_t struct.
// In that case, when running on Windows or using a libstdc++ >= 10, we just alias the std::same_as and std::remove_cvref_t symbols,
// Otherwise, we just implement them.
#if defined(_WINDOWS) || (defined(_GLIBCXX_RELEASE) && _GLIBCXX_RELEASE >= 10 )

template <class T, class U>
concept same_as = std::same_as<T, U>;

template <class T>
using remove_cvref_t = typename std::remove_cvref_t<T>;

#else

template<class T>
struct remove_cvref
{
    typedef std::remove_cv_t<std::remove_reference_t<T>> type;
};

template< class T >
using remove_cvref_t = typename remove_cvref<T>::type;

namespace detail
{
    template< class T, class U >
    concept SameHelper = std::is_same_v<T, U>;
}

template< class T, class U >
concept same_as = detail::SameHelper<T, U> && detail::SameHelper<U, T>;

#endif

template <class T>
concept IsWstring = same_as<T, ::shared::WSTRING> ||
                    // check if it's WCHAR[N] or WCHAR*
                    same_as<remove_cvref_t<std::remove_pointer_t<std::decay_t<T>>>, WCHAR>;
template <IsWstring T>
void WriteToStream(std::ostringstream& oss, T const& x)
{
    if constexpr (std::is_same_v<T, ::shared::WSTRING>)
    {
        oss << ::shared::ToString(x);
    }
    else if constexpr (std::is_array_v<T>)
    {
        oss << ::shared::ToString(x, std::extent_v<T>);
    }
    else
    {
        oss << ::shared::ToString(x);
    }
}

template <class Period>
const char* time_unit_str()
{
    if constexpr(std::is_same_v<Period, std::nano>)
    {
        return "ns";
    }
    else if constexpr(std::is_same_v<Period, std::micro>)
    {
        return "us";
    }
    else if constexpr(std::is_same_v<Period, std::milli>)
    {
        return "ms";
    }
    else if constexpr(std::is_same_v<Period, std::ratio<1>>)
    {
        return "s";
    }

    return "<unknown unit of time>";
}

template <class Rep, class Period>
void WriteToStream(std::ostringstream& oss, std::chrono::duration<Rep, Period> const& x)
{
    oss << x.count() << time_unit_str<Period>();
}

template <class T>
void WriteToStream(std::ostringstream& oss, T const& x)
{
    oss << x;
}

template <typename... Args>
static std::string LogToString(Args const&... args)
{
    std::ostringstream oss;
    (WriteToStream(oss, args), ...);

    return oss.str();
}

template <typename... Args>
void Logger::Debug(const Args&... args)
{
    if (IsDebugEnabled())
    {
        _internalLogger->debug(LogToString(args...));
    }
}

template <typename... Args>
void Logger::Info(const Args&... args)
{
    _internalLogger->info(LogToString(args...));
}

template <typename... Args>
void Logger::Warn(const Args&... args)
{
    _internalLogger->warn(LogToString(args...));
}

template <typename... Args>
void Logger::Error(const Args&... args)
{
    _internalLogger->error(LogToString(args...));
}

template <typename... Args>
void Logger::Critical(const Args&... args)
{
    _internalLogger->critical(LogToString(args...));
}

inline void Logger::Flush()
{
    _internalLogger->flush();
}

inline void Logger::EnableDebug(bool enable)
{
    m_debug_logging_enabled = enable;
}

inline bool Logger::IsDebugEnabled() const
{
    return m_debug_logging_enabled;
}
} // namespace datadog::shared
