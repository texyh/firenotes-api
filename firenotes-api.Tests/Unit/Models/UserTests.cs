﻿using firenotes_api.Models.Data;
using FluentAssertions;
using Xunit;

namespace firenotes_api.Tests.Models
{
    public class UserTests
    {
        [Fact]
        public void ShouldAutoGenerateId()
        {
            var user = new User();
            user.Id.Should().NotBeNullOrWhiteSpace();
        }
    }
}