using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAPIWrapper
{
    public class FaceAPIClient
    {

        private FaceServiceClient _faceClient;
        private string _groupId;

        public FaceAPIClient(string serviceKey, string serviceRoot, string groupId)
        {
            _faceClient = new FaceServiceClient(serviceKey, serviceRoot);
            _groupId = groupId;
          
        }

        public Face[] DetectFaces(string imageUrl)
        {
            Task<Face[]> task = Task.Run<Face[]>( async () => await _faceClient.DetectAsync(imageUrl));
            return task.Result;

        }

        public Face[] DetectFaces(Stream s)
        {
            Task<Face[]> task = Task.Run<Face[]>(async () => await _faceClient.DetectAsync(s));
            return task.Result;

        }

        public void CreatePersonGroup(string groupName)
        {
            Task.Run( async () => await _faceClient.CreatePersonGroupAsync(_groupId.ToLower(), groupName.ToLower()));

        }

        public PersonGroup GetPersonGroup()
        {
            Task<PersonGroup> task = Task.Run<PersonGroup>(async () => await _faceClient.GetPersonGroupAsync(_groupId));
            return task.Result;

        }

        public CreatePersonResult CreatePerson(string name, string userData)
        {
            try { 
            Task<CreatePersonResult> task = Task.Run<CreatePersonResult>(async() => await _faceClient.CreatePersonAsync(_groupId.ToLower(), name.ToLower(), userData));
            return task.Result;
            }
            catch (FaceAPIException e)
            {
                return null;
            }
        }

        public void AddFacesToPerson(Guid personId, string userData, string[] urls )
        {
            foreach( string url in urls)
            {
                try
                {
                    FaceRectangle faceRect = DetectFaces(url)[0].FaceRectangle;

                    Task<AddPersistedFaceResult> task = Task.Run<AddPersistedFaceResult>(async () => await _faceClient.AddPersonFaceAsync(_groupId, personId, url, userData, faceRect));
                }
                catch { }
            }
        }

        public void TrainPersonGroup()
        {
            Task.Run( async () => await _faceClient.TrainPersonGroupAsync(_groupId));
        }

        public IdentifyResult[] Identify(Guid[] faceIds)
        {
            try
            {
                Task<IdentifyResult[]> task = Task.Run<IdentifyResult[]>(async () => await _faceClient.IdentifyAsync(_groupId, faceIds, 1));
                return task.Result;
            }
            catch (FaceAPIException e)
            {
                return null;
            }
        }

        public Guid IdentifyFaceFromFile(string imgFilePath)
        {
            try
            {
                using (Stream s = File.OpenRead(imgFilePath))
                {
                    var faces = DetectFaces(s);

                    if (faces == null || faces.Length == 0)
                    {
                        return Guid.Empty;
                    }
                    var faceIds = faces.Select(Face => Face.FaceId).ToArray();

                    IdentifyResult[] results = Identify(faceIds);

                    if (results == null || results.Length == 0)
                    {
                        return Guid.Empty;
                    }

                    return results.FirstOrDefault().Candidates.FirstOrDefault().PersonId;
                }
            }
            catch { return Guid.Empty; }
        }

        public Guid IdentifyFaceFromStream(Stream s)
        {
            try
            {
                var faces = DetectFaces(s);

                if (faces == null || faces.Length == 0)
                {
                    return Guid.Empty;
                }

                var faceIds = faces.Select(Face => Face.FaceId).ToArray();

                IdentifyResult[] results = Identify(faceIds);

                if (results == null || results.Length == 0)
                {
                    return Guid.Empty;
                }

                return results.FirstOrDefault().Candidates.FirstOrDefault().PersonId;
            }
            catch
            {
                return Guid.Empty;
            }
            
        }






    }
}
