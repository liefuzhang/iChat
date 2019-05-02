using iChat.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iChat.UnitTests.Services
{
    [TestClass]
    public class MessageParsingServiceTest
    {
        private MessageParsingService _messageParsingService;

        [TestInitialize]
        public void TestInitialize()
        {
            _messageParsingService = new MessageParsingService();
        }

        [DataTestMethod]
        [DataRow("*hello*", "<b>hello</b>")]
        [DataRow("_hello_", "<i>hello</i>")]
        [DataRow("~hello~", "<strike>hello</strike>")]
        [DataRow("`hello`", "<code>hello</code>")]
        [DataRow("```hello```", "<pre>hello</pre>")]
        [DataRow("_*hello*_", "<i><b>hello</b></i>")]
        [DataRow("```*hello*```", "<pre>*hello*</pre>")]
        [DataRow("```<p>hello</p>text```", "<pre><p>hello</p>text</pre>")]
        [DataRow("*_hello_*", "<b><i>hello</i></b>")]
        [DataRow("~*_hello_*~", "~<b><i>hello</i></b>~")]
        [DataRow("_*hello_*", "<i>*hello</i>*")]
        [DataRow("_**_", "<i>**</i>")]
        [DataRow("******", "******")]
        [DataRow("_str1*hello*_", "_str1<b>hello</b>_")]
        [DataRow("something**", "something**")]
        [DataRow("something *test *", "something *test *")]
        [DataRow("<p>line 1</p><p>line2*hello*_</p>", "<p>line 1</p><p>line2<b>hello</b>_</p>")]
        [DataRow("<p>line *1</p><p>line2*</p>", "<p>line *1</p><p>line2*</p>")]
        public void Parse_WhenCalled_ReturnParsedHtml(string input, string expectedHtml)
        {
            // arrange

            // act
            var result = _messageParsingService.Parse(input);

            // assert
           Assert.AreEqual(expectedHtml, result);
        }

        public void Parse_WhenInputIsNull_ReturnEmptyString()
        {
            // arrange

            // act
            var result = _messageParsingService.Parse(null);

            // assert
            Assert.AreEqual(string.Empty, result);
        }
    }
}
