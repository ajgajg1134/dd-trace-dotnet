﻿using System;
using System.Diagnostics;

namespace Datadog.Util
{
    internal static class TimeScale
    {
        public const int NanosecsInMillisec_Int32 = 1000000;
        public const int NanosecsInSec_Int32 = 1000000000;
        public const int MillisecsInSec_Int32 = 1000;

        public const ulong NanosecsInMillisec_UInt64 = NanosecsInMillisec_Int32;
        public const ulong NanosecsInSec_UInt64 = NanosecsInSec_Int32;
        public const ulong MillisecsInSec_UInt64 = MillisecsInSec_Int32;

        public const double NanosecsInMillisec_Double = (double) NanosecsInMillisec_Int32;
        public const double NanosecsInSec_Double = (double) NanosecsInSec_Int32;
        public const double MillisecsInSec_Double = (double) MillisecsInSec_Int32;

        public static readonly double StopwatchTicksPerNanosec = Stopwatch.Frequency / NanosecsInSec_Double;

        public static ulong MsToNs(ulong millisecs)
        {
            return millisecs * NanosecsInMillisec_UInt64;
        }

        public static double NsToSec(ulong nanosecs)
        {
            return (nanosecs / NanosecsInSec_Double);
        }

        public static double NsToSec(ulong nanosecs, int roundToDecimals)
        {
            return Math.Round(nanosecs / NanosecsInSec_Double, roundToDecimals);
        }

        public static double NsToMs(ulong nanosecs)
        {
            return (nanosecs / NanosecsInMillisec_Double);
        }

        public static double NsToMs(ulong nanosecs, int roundToDecimals)
        {
            return Math.Round(nanosecs / NanosecsInMillisec_Double, roundToDecimals);
        }

        public static long GetElapsedNanoseconds(this Stopwatch timer)
        {
            if (timer == null)
            {
                return 0;
            }
            else
            {
                long ticks;
                unchecked
                {
                    ticks = timer.ElapsedTicks;
                }

                return (long) Math.Round(ticks / StopwatchTicksPerNanosec);
            }
        }
    }
}
