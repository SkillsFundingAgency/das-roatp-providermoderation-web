﻿using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Roatp.CourseManagement.Domain.Configuration
{
    [ExcludeFromCodeCoverage]
    public class RoatpProviderModerationOuterApiConfiguration
    {
        public string SubscriptionKey { get; set; }
        public string BaseUrl { get; set; }
    }
}
