using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }

        [Test]
        public void EnterEmail()
        {
            var emailField = "text_email";
            var notifyField = "button_notify";

            app.WaitForElement(emailField);
            if (platform == Platform.Android)
            {
                app.ClearText(emailField);
            }
            var query = app.Query(notifyField);
            Assert.IsFalse(query[0].Enabled);

            app.EnterText(emailField, "mike@xamarin.com");
            app.Screenshot("Then I enter an email address correct.");
            query = app.Query(notifyField);
            Assert.IsTrue(query[0].Enabled);
            app.Screenshot("Notify Enabled");
            app.ClearText(emailField);
            app.Screenshot("Then I clear the email address.");

            app.EnterText(emailField, "mike@xamarincom");
            app.Screenshot("Then I enter an email address incorrect.");
            query = app.Query(notifyField);
            Assert.IsFalse(query[0].Enabled);

        }

        [Test]
        public void PressNotify()
        {
            var emailField = "text_email";
            var notifyField = "button_notify";
            app.WaitForElement(emailField);

            if (platform == Platform.Android)
            {
                app.ClearText(emailField);
            }
            app.EnterText(emailField, "mike@xamarin.com");
            app.Screenshot("Then I enter an email address");

            var button = app.Query(notifyField);
            if (button.First().Enabled)
                app.Screenshot("then Notify becomes enabled");
            else
                Assert.Fail();

            app.DismissKeyboard();

            app.Tap(notifyField);

            if(platform == Platform.Android)
                app.WaitForElement(x => x.Marked("OK"));
            else
                app.WaitForElement(x => x.Text("All Set!"));
                
            app.Screenshot("Then I signup");

            app.Tap(x => x.Marked("OK"));

            app.DismissKeyboard();
        }

        [Test]
        public void AddToCalendar()
        {
            
            var addToCalendar = "button_calendar";

            app.WaitForElement(addToCalendar);
            app.Screenshot("Wait for add to calendar");

            app.Tap(addToCalendar);

            /*if (this.platform == Platform.iOS)
            {
                app.Repl();
                app.WaitForElement(x => x.Text("OK"));
                app.Screenshot("Then I'm asked for permission");

                app.Tap(x => x.Marked("OK"));
            }*/

            app.WaitForElement(x => x.Marked("Added to Calendar Successfully"));
            app.Screenshot("Then the date is added to my calendar");
        }

        [Test]
        public void Share()
        {
            var shareButton = "button_share";

            app.WaitForElement(shareButton);
            app.Screenshot("Wait for Share...");

            app.Tap(shareButton);
            //app.Repl();
            if (platform == Platform.iOS)
            {
                var waitFor = platform == Platform.iOS ? "Cancel" : "Share Event";
                app.WaitForElement(x => x.Marked(waitFor));
            }
            //app.Screenshot("Then the share is visible");

        }
    }
}

