using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FaceAPIWrapper;
using Microsoft.ProjectOxford.Face.Contract;

namespace FaceAPIWrapperUnitTests
{
    [TestClass]
    public class FaceAPIWrapperTests
    {

        public const string GROUP_ID = "nick12345";
        public const string SERVICE_KEY = "";
        public const string SERVICE_URL = "";
        [TestMethod]
        public void TestClassCreation()
        {
            FaceAPIClient client = new FaceAPIClient(SERVICE_KEY, SERVICE_URL, GROUP_ID);

            Assert.IsNotNull(client);
        }


        [TestMethod]
        public void TestCreatePersonGroup()
        {
            //FaceAPIClient client = new FaceAPIClient(SERVICE_KEY, SERVICE_URL, GROUP_ID);
            //client.CreatePersonGroup("nick12345", "mynewGroupName");

        }

        [TestMethod]
        public void TestGetPersonGroup()
        {
            FaceAPIClient client = new FaceAPIClient(SERVICE_KEY, SERVICE_URL, GROUP_ID);


            var pg = client.GetPersonGroup();
         
        }
        [TestMethod]
        public void TestCreatePerson()
        {
            FaceAPIClient client = new FaceAPIClient(SERVICE_KEY, SERVICE_URL, GROUP_ID);
            var pgr = client.CreatePerson( "Nick Giard", "nmgiard");

        }

        [TestMethod]
        public void TestCreatePerson2()
        {
            FaceAPIClient client = new FaceAPIClient(SERVICE_KEY, SERVICE_URL, GROUP_ID);
            var pgr = client.CreatePerson("Heather Giard", "hegiard");

        }

        [TestMethod]
        public void TestDetectFace()
        {
            FaceAPIClient client = new FaceAPIClient(SERVICE_KEY, SERVICE_URL, GROUP_ID);
            Face[] faces = client.DetectFaces("https://facescanblob1.blob.core.usgovcloudapi.net/facescan-directory/nmgiard/nick1.png");
            Assert.AreEqual(faces.Length, 1);
            

        }

        [TestMethod]
        public void TestAddFacesToPerson()
        {
            FaceAPIClient client = new FaceAPIClient(SERVICE_KEY, SERVICE_URL, GROUP_ID);
            string[] urls = new string[] { "https://facescanblob1.blob.core.usgovcloudapi.net/facescan-directory/nmgiard/nick1.png", "https://facescanblob1.blob.core.usgovcloudapi.net/facescan-directory/nmgiard/nick2.jpg", "https://facescanblob1.blob.core.usgovcloudapi.net/facescan-directory/nmgiard/nick3.jpg" };
            client.AddFacesToPerson( new Guid("fa4f14ff-17ea-4095-afa6-2cc1f56e7b6c"), "nmgiard", urls);

        }

        [TestMethod]
        public void TestAddFacesToPerson2()
        {
            FaceAPIClient client = new FaceAPIClient(SERVICE_KEY, SERVICE_URL, GROUP_ID);
            string[] urls = new string[] { "https://facescanblob1.blob.core.usgovcloudapi.net/facescan-directory/hegiard/heather3.jpg"};
            client.AddFacesToPerson(new Guid("ac269da7-7434-4a42-9c40-3bdf766699c1"), "hegiard", urls);

        }

        [TestMethod]
        public void TestTrainPersonGroup()
        {
            FaceAPIClient client = new FaceAPIClient(SERVICE_KEY, SERVICE_URL, GROUP_ID);
            client.TrainPersonGroup();
        }

        [TestMethod]
        public void TestIdentify()
        {
            FaceAPIClient client = new FaceAPIClient(SERVICE_KEY, SERVICE_URL, GROUP_ID);
            var person = client.IdentifyFaceFromFile( "C:\\Users\\nmgiard\\Desktop\\nick.jpg");
            Assert.AreEqual(new Guid("fa4f14ff-17ea-4095-afa6-2cc1f56e7b6c"), person);
        }

        [TestMethod]
        public void TestIdentify2()
        {
            FaceAPIClient client = new FaceAPIClient(SERVICE_KEY, SERVICE_URL, GROUP_ID);
            var person = client.IdentifyFaceFromFile("C:\\Users\\nmgiard\\Desktop\\heather_test.jpg");
            Assert.AreEqual(new Guid("ac269da7-7434-4a42-9c40-3bdf766699c1"), person);
        }
    }
}
