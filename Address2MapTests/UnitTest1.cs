using Address2Map.BusinessController;
using Address2Map.Repository;
using Address2MapTests.NUnitTestCiscoService;
using Microsoft.Extensions.DependencyInjection;

namespace Address2MapTests
{
    public class Tests
    {
        public MockWebApp web;
        public HttpClient client;

        [SetUp]
        public void Setup()
        {
            web = new MockWebApp();
            client = web.CreateClient();
            var rujan = web.Services.GetService<RuianRepository>();
            rujan?.ProcessCSV(File.ReadAllBytes("Data/20220930_OB_554782_ADR.csv"));
        }

        [Test]
        public void ConversionNabreziTest()
        {
            var scope = web?.Services?.CreateScope();
            var addrController = scope?.ServiceProvider?.GetService(typeof(AddressBusinessController)) as AddressBusinessController;
            Assert.That(addrController, Is.Not.Null);
            var input = @"Ra��novo n�b�. - sud� �. 28-50";
            var ret = addrController.ProcessText2Output(554782, input);
            Assert.That(ret.Input.Trim(), Is.EqualTo(input));
            Assert.That(ret.Output.Trim(), Is.EqualTo("Ra��novo n�b�e�� - sud� �. 28-50"));
            Assert.That(ret.Notes.Trim(), Is.EqualTo("We suggest to change street name"));
        }
        [Test]
        public void ConversionDashTest()
        {
            var scope = web?.Services?.CreateScope();
            var addrController = scope?.ServiceProvider?.GetService(typeof(AddressBusinessController)) as AddressBusinessController;
            Assert.That(addrController, Is.Not.Null);
            char chDash = '\u2013';

            var input = @$"Zlat� uli�ka u Daliborky {chDash} sud� �. 28�50";
            var ret = addrController.ProcessText2Output(554782, input);
            Assert.That(ret.Input.Trim(), Is.EqualTo(input));
            Assert.That(ret.Output.Trim(), Is.EqualTo("Zlat� uli�ka u Daliborky - sud� �. 28-50"));
            Assert.That(ret.Notes.Trim(), Is.EqualTo("UTF Dash has been replaced with hyphen"));
        }
    }
}