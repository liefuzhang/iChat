using iChat.Api.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iChat.UnitTests.Helpers
{
    [TestClass]
    public class MessageParsingServiceTest
    {
        private MessageParsingHelper _messageParsingService;

        [TestInitialize]
        public void TestInitialize()
        {
            _messageParsingService = new MessageParsingHelper();
        }

        [DataTestMethod]
        [DataRow("*hello*", "<b>hello</b>")]
        [DataRow("_hello_", "<i>hello</i>")]
        [DataRow("~hello~", "<strike>hello</strike>")]
        [DataRow("`hello`", "<code>hello</code>")]
        [DataRow("_*hello*_", "<i><b>hello</b></i>")]
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
        [DataRow("<p>```hello```</p>", "<pre>hello</pre>")]
        [DataRow("<p>```*hello*```</p>", "<pre>*hello*</pre>")]
        [DataRow("<p>```test```test```</p>", "<pre>test```test</pre>")]
        [DataRow("<p>```<p>hello</p>text```</p>", "<pre><p>hello</p>text</pre>")]
        [DataRow("<p>&gt;hello</p>", "<blockquote>hello</blockquote>")]
        [DataRow("<p>&gt;hello</p><p>&gt;hello</p>", "<blockquote>hello</blockquote><blockquote>hello</blockquote>")]
        [DataRow("<p>&gt;*hello*</p>", "<blockquote><b>hello</b></blockquote>")]
        [DataRow("<p>test&gt;hello</p>", "<p>test&gt;hello</p>")]
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
