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
            var emailField = platform == Platform.Android ? "text_email" : "Email Address";
            var notifyField = platform == Platform.Android ? "button_notify" : "Notify Me";
            app.WaitForElement(x => x.Marked(emailField));
            if (platform == Platform.Android)
            {
                app.ClearText(x => x.Marked(emailField));
            }
            var query = app.Query(x => x.Marked(notifyField));
            Assert.IsFalse(query[0].Enabled);

            app.EnterText(x => x.Marked(emailField), "mike@xamarin.com");
            app.Screenshot("Then I enter an email address correct.");
            query = app.Query(x => x.Marked(notifyField));
            Assert.IsTrue(query[0].Enabled);
            app.Screenshot("Notify Enabled");
            app.ClearText(x => x.Text("mike@xamarin.com"));
            app.Screenshot("Then I clear the email address.");

            app.EnterText(x => x.Marked(emailField), "mike@xamarincom");
            app.Screenshot("Then I enter an email address incorrect.");
            query = app.Query(x => x.Marked(notifyField));
            Assert.IsFalse(query[0].Enabled);

        }

        [Test]
        public void PressNotify()
        {
            var emailField = platform == Platform.Android ? "text_email" : "Email Address";
            var notifyField = platform == Platform.Android ? "button_notify" : "Notify Me";
            app.WaitForElement(x => x.Marked(emailField));

            if (platform == Platform.Android)
            {
                app.ClearText(x => x.Marked(emailField));
            }
            app.EnterText(x => x.Marked(emailField), "mike@xamarin.com");
            app.Screenshot("Then I enter an email address");

            var button = app.Query(x => x.Marked(notifyField));
            if (button.First().Enabled)
                app.Screenshot("then Notify becomes enabled");
            else
                Assert.Fail();

            app.DismissKeyboard();

            app.Tap(x => x.Marked(notifyField));

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
            
            
            app.WaitForElement(x => x.Marked("Add to Calendar"));
            app.Screenshot("Wait for add to calendar");

            app.Tap(x => x.Marked("Add to Calendar"));

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
            app.WaitForElement(x => x.Marked("Share..."));
            app.Screenshot("Wait for Share...");

            app.Tap(x => x.Marked("Share..."));
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

