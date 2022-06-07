FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019

# Add the .NET Tracer MSI
ARG DOTNET_TRACER_MSI
COPY $DOTNET_TRACER_MSI ./datadog-apm.msi

# Install the MSI (will do this when already running)
#RUN Start-Process -Wait msiexec -ArgumentList '/qn /i datadog-apm.msi'

ENTRYPOINT ["cmd", "/c", "ping -t localhost > NUL"]