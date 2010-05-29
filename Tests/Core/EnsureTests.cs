using System;
using Xunit;
using Xunit.Extensions;
using MultiTenancy.Core;

namespace MultiTenancy.Tests.Core
{
    public class EnsureTests
    {
        [Fact]
        public void Ensure_That_DoesNotThrowException_WhenExpressionTrue()
        {
            Assert.DoesNotThrow(() => Ensure.That(true));
        }

        [Fact]
        public void Ensure_That_ThrowsException_WhenExpressionFalse()
        {
            Assert.Throws<Exception>(() => Ensure.That(false));
        }

        [Fact]
        public void Ensure_That_WithFalseConditionAndMessage_ThrowsExceptionWithMessage()
        {
            string message = "%";
            try
            { 
                Ensure.That(false, message);
                Assert.True(false, "The Ensure.That exception was not thrown");
            }
            catch (Exception ex)
            {
                Assert.Equal(message, ex.Message);
            }
        }

        [Fact]
        public void Ensure_That_WithGenericExceptionTypeAndFalseCondition_ThrowsSpecificException()
        {
            Assert.Throws<InvalidTimeZoneException>(() => Ensure.That<InvalidTimeZoneException>(false));
        }

        [Fact]
        public void Ensure_That_WithGenericExceptionTypeAndTrueCondition_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => Ensure.That<InvalidTimeZoneException>(true));
        }

        [Fact]
        public void Ensure_That_WithFalseConditionAndMessageAndGenericException_ThrowsExceptionWithMessage()
        {
            string message = "%";
            try
            {
                Ensure.That<InvalidTimeZoneException>(false, message);
                Assert.True(false, "The Ensure.That exception was not thrown");
            }
            catch (InvalidTimeZoneException ex)
            {
                Assert.Equal(message, ex.Message);
            }
            catch (Exception)
            {
                Assert.True(false, "The Ensure.That exception was not correct type");
            }
        }

        [Fact]
        public void Ensure_Not_FalseCondition_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => Ensure.Not<Exception>(false));
        }

        [Fact]
        public void Ensure_Not_TrueCondition_ThrowsSpecifiedException()
        {
            Assert.Throws<InvalidTimeZoneException>(() => Ensure.Not<InvalidTimeZoneException>(true));
        }

        [Fact]
        public void Ensure_Not_WithTrueCondition_AndMessage_AndGenericException_ThrowsExceptionWithMessage()
        {
            string message = "%";
            try
            {
                Ensure.Not<InvalidTimeZoneException>(true, message);
                Assert.True(false, "The Ensure.Not exception was not thrown");
            }
            catch (InvalidTimeZoneException ex)
            {
                Assert.Equal(message, ex.Message);
            }
            catch (Exception)
            {
                Assert.True(false, "The Ensure.Not exception was not correct type");
            }
        }

        [Fact]
        public void Ensure_Not_NonGeneric_WithFalseCondition_AndNoMessase_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => Ensure.Not(false));
        }

        [Fact]
        public void Ensure_Not_NonGeneric_WithFalseCondition_AndMessase_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => Ensure.Not(false, "message"));
        }

        [Fact]
        public void Ensure_Not_NonGeneric_WithTrueCondition_AndNoMessage_ThrowsException()
        {
            Assert.Throws<Exception>(() => Ensure.Not(true));
        }

        [Fact]
        public void Ensure_Not_NonGeneric_WithTrueValueAndMessage_ThrowsExceptionWithCorrectMessage()
        {
            string message = "%";
            try
            {
                Ensure.Not(true, message);
                Assert.True(false, "The Ensure.Not exception was not thrown");
            }
            catch (Exception ex)
            {
                Assert.Equal(message, ex.Message);
            }
        }

        [Fact]
        public void Ensure_NotNull_WithNonNullObject_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => Ensure.NotNull(new object()));
        }

        [Fact]
        public void Ensure_NotNull_WithNullObject_ThrowNullReferenceExceptionException()
        {
            Assert.Throws<NullReferenceException>(() => Ensure.NotNull(null));
        }

        [Fact]
        public void Ensure_NotNull_WithNullValueAndMessage_ThrowsNullReferenceExceptionWithCorrectMessage()
        {
            string message = "%";
            try
            {   
                Ensure.NotNull(null, message);
                Assert.True(false, "The Ensure.NotNull exception was not thrown");
            }
            catch (NullReferenceException ex)
            {
                Assert.Equal(message, ex.Message);
            }
            catch (Exception)
            {
                Assert.True(false, "The Ensure.NotNull exception was not correct type");
            }
        }

        [Fact]
        public void Ensure_Equal_WithEqualValues_DoesNotThrowException()
        { 
            string value = "%";
            Assert.DoesNotThrow(() => Ensure.Equal(value, value));
        }

        [Fact]
        public void Ensure_Equal_WithEqualValues_AndMessage_DoesNotThrowException()
        {
            string value = "%";
            Assert.DoesNotThrow(() => Ensure.Equal(value, value, "message"));
        }

        [Fact]
        public void Ensure_Equal_WithNonEqualValues_ThrowsException()
        {
            string value1 = "%", value2 = "$";
            Assert.Throws<Exception>(() => Ensure.Equal(value1, value2));
        }

        [Fact]
        public void Ensure_Equal_WithNonEqualValuesAndMessage_ThrowsExceptionWithCorrectMessage()
        {
            string message = "%";
            try
            {
                Ensure.Equal("a", "b", message);
                Assert.True(false, "The Ensure.Equal exception was not thrown");
            }
            catch (Exception ex)
            {
                Assert.Equal(message, ex.Message);
            }
        }

        [Fact]
        public void Ensure_Equal_GivenNullLeftValue_ThrowsException()
        {
            Assert.Throws<Exception>(() => Ensure.Equal<string>(null, "%"));
        }

        [Fact]
        public void Ensure_Equal_GivenNullRightValue_ThrowsException()
        {
            Assert.Throws<Exception>(() => Ensure.Equal<string>("^", null));
        }

        [Fact]
        public void Ensure_NotEqual_GivenEqualValues_ThrowsException()
        {
            Assert.Throws<Exception>(() => Ensure.NotEqual("$", "$"));
        }

        [Fact]
        public void Ensure_NotEqual_GivenNonEqualValues_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => Ensure.NotEqual("$", "%"));
        }

        [Fact]
        public void Ensure_NotEqual_GivenNullLeftValues_ThrowsException()
        {
            Assert.Throws<Exception>(() => Ensure.NotEqual<string>(null, "#"));
        }

        [Fact]
        public void Ensure_NotEqual_GivenNullRightValues_ThrowsException()
        {
            Assert.Throws<Exception>(() => Ensure.NotEqual<string>("#", null));
        }

        [Fact]
        public void Ensure_NotEqual_WithEqualValuesAndMessage_ThrowsExceptionWithCorrectMessage()
        {
            string message = "%";
            try
            {
                Ensure.NotEqual("a", "a", message);
                Assert.True(false, "The Ensure.NotEqual exception was not thrown");
            }
            catch (Exception ex)
            {
                Assert.Equal(message, ex.Message);
            }
        }

        [Fact]
        public void Ensure_Contains_GivenNullCollection_ThrowsException()
        {
            Assert.Throws<Exception>(() => Ensure.Contains<string>(null, x => true));
        }

        [Fact]
        public void Ensure_Contains_GivenBlankCollection_ThrowsException()
        {
            Assert.Throws<Exception>(() => Ensure.Contains<string>(new string[0], x => true));
        }

        [Fact]
        public void Ensure_Contains_GivenCollectionWithoutSatisfyingValue_ThrowsException()
        {
            Assert.Throws<Exception>(() => Ensure.Contains<string>(new string[] { "hello", "world" }, x => false));
        }

        [Fact]
        public void Ensure_Contains_GivenCollectionWithSatisfyingValue_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => Ensure.Contains<string>(new string[] { "hello", "world" }, x => true));
        }

        [Fact]
        public void Ensure_Contains_WithEmptyCollectionAndMessage_ThrowsExceptionWithCorrectMessage()
        {
            string message = "%";
            try
            {
                Ensure.Contains<string>(null, x => true, message);
                Assert.True(false, "The Ensure.Contains exception was not thrown");
            }
            catch (Exception ex)
            {
                Assert.Equal(message, ex.Message);
            }
        }

        [Fact]
        public void Ensure_NotNullOrEmpty_DoesNotThrowException_GivenNonEmptyString()
        {
            Assert.DoesNotThrow(() => Ensure.NotNullOrEmpty("a"));
        }

        [Fact]
        public void Ensure_NotNullOrEmpty_ThrowsException_GivenEmptyString()
        {
            Assert.Throws<Exception>(() => Ensure.NotNullOrEmpty(""));
        }

        [Fact]
        public void Ensure_NotNullOrEmpty_ThrowsException_GivenNullString()
        {
            Assert.Throws<Exception>(() => Ensure.NotNullOrEmpty(""));
        }

        [Fact]
        public void Ensure_NotNullOfEmpty_GivenNullStringAndMessage_ThrowsExceptionWithCorrectMessage()
        {
            string message = "%";
            try
            {
                Ensure.NotNullOrEmpty(null, message);
                Assert.True(false, "The Ensure.NotNullOrEmpty exception was not thrown");
            }
            catch (Exception ex)
            {
                Assert.Equal(message, ex.Message);
            }
        }

        [Fact]
        public void Ensure_Argument_Is_GivenFalseCondition_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Ensure.Argument.Is(false));
        }

        [Fact]
        public void Ensure_Argument_Is_GivenTrueCondition_DoesNotThrowArgumentException()
        {
            Assert.DoesNotThrow(() => Ensure.Argument.Is(true));
        }

        [Fact]
        public void Ensure_Argument_Is_GivenFalseConditionAndMessage_ThrowsExceptionWithCorrectMessage()
        {
            string message = "%";
            try
            {
                Ensure.Argument.Is(false, message);
                Assert.True(false, "The Ensure.Argument.Is exception was not thrown");
            }
            catch (Exception ex)
            {
                Assert.Equal(message, ex.Message);
            }
        }

        [Fact]
        public void Ensure_Argument_IsNot_GivenTrueCondition_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Ensure.Argument.IsNot(true));
        }

        [Fact]
        public void Ensure_Argument_Is_GivenFalseCondition_DoesNotThrowArgumentException()
        {
            Assert.DoesNotThrow(() => Ensure.Argument.IsNot(false));
        }

        [Fact]
        public void Ensure_Argument_IsNot_GivenTrueConditionAndMessage_ThrowsExceptionWithCorrectMessage()
        {
            string message = "%";
            try
            {
                Ensure.Argument.IsNot(true, message);
                Assert.True(false, "The Ensure.Argument.IsNot exception was not thrown");
            }
            catch (Exception ex)
            {
                Assert.Equal(message, ex.Message);
            }
        }

        [Fact]
        public void Ensure_Argument_IsNot_GivenNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => Ensure.Argument.NotNull(null));
        }

        [Fact]
        public void Ensure_Argument_Is_GivenNonNullCondition_DoesNotThrowArgumentException()
        {
            Assert.DoesNotThrow(() => Ensure.Argument.NotNull(false));
        }

        [Fact]
        public void Ensure_Argument_IsNot_GivenNullValueAndMessage_ThrowsExceptionWithCorrectMessage()
        {
            string param = "%";
            try
            {
                Ensure.Argument.NotNull(null, param);
                Assert.True(false, "The Ensure.Argument.NotNull exception was not thrown");
            }
            catch (ArgumentNullException ex)
            {
                Assert.Equal(param, ex.ParamName);
            }
        }

        [Fact]
        public void Ensure_Argument_NotNullOrEmpty_GivenNonEmptyString_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => Ensure.Argument.NotNullOrEmpty("s"));
        }

        [Fact]
        public void Ensure_Argument_NotNullOrEmpty_ThrowsArgumentException_WhenGivenNullSting()
        {
            Assert.Throws<ArgumentException>(() => Ensure.Argument.NotNullOrEmpty(null));
        }

        [Fact]
        public void Ensure_Argument_NotNullOrEmpty_ThrowsArgumentException_WhenGivenEmptySting()
        {
            Assert.Throws<ArgumentException>(() => Ensure.Argument.NotNullOrEmpty(""));
        }

        [Fact]
        public void Ensure_Argument_NotNullOrEmpty_ThrowsArgumentException_WhenGivenNullSting_AndParameterName()
        {
            Assert.Throws<ArgumentException>(() => Ensure.Argument.NotNullOrEmpty(null, "param"));
        }

        [Fact]
        public void Ensure_Argument_NotNullOrEmpty_ThrowsArgumentException_WhenGivenEmptySting_AndParameterName()
        {
            Assert.Throws<ArgumentException>(() => Ensure.Argument.NotNullOrEmpty("", "param"));
        }

        [Fact]
        public void Ensure_Argument_InFuture_DoesntThrowException_WhenGivenDateTimeInFuture()
        {
            Assert.DoesNotThrow(() => Ensure.Argument.InFuture(DateTime.Now.AddMinutes(1)));
        }

        [Fact]
        public void Ensure_Argument_InFuture_ThrowsException_WhenGivenDateTimeInPast()
        {
            Assert.Throws<ArgumentException>(() => Ensure.Argument.InFuture(DateTime.Now.AddMinutes(-1)));
        }

        [Theory]
        [InlineData("http://www.google.com/")]
        [InlineData("http://www.google.com")]
        [InlineData("http://google.com")]
        [InlineData("http://google.com/")]
        [InlineData("https://www.google.com/")]
        [InlineData("https://www.google.com/something")]
        [InlineData("https://www.google.com/something-cool")]
        [InlineData("https://www.google.com?q=hello")]
        public void Ensure_Argument_IsUrl_DoesntThrown_GivenProperlyFormedUrl(string value)
        {
            Assert.DoesNotThrow(() => Ensure.Argument.IsUrl(value));
        }

        [Theory]
        [InlineData("http:/google.com")]
        [InlineData("http:google.com")]
        [InlineData("httpgoogle.com")]
        [InlineData("google.com")]
        [InlineData(".com")]
        [InlineData("somepage")]
        [InlineData("somepage.aspx")]
        [InlineData("")]
        [InlineData(null)]
        public void Ensure_Argument_IsUrl_ThrowsException_GivenMalformedUrl(string value)
        {
            Assert.Throws<ArgumentException>(() => Ensure.Argument.IsUrl(value));
        }
    }
}