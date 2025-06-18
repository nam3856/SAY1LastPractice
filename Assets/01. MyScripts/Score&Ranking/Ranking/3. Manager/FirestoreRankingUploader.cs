using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine;

public class FirestoreRankingUploader
{
    private readonly FirebaseFirestore _db;
    private const string COLLECTION_NAME = "rankings";

    public FirestoreRankingUploader()
    {
        _db = FirebaseFirestore.DefaultInstance;
    }

    public async Task UploadIfHighScoreAsync(ScoreDTO scoreDTO)
    {
        var docRef = _db.Collection(COLLECTION_NAME).Document(scoreDTO.PlayerId);
        var snapshot = await docRef.GetSnapshotAsync();

        bool shouldUpload = false;

        if (!snapshot.Exists)
        {
            shouldUpload = true;
        }
        else
        {
            var current = snapshot.ConvertTo<RankingEntry>();
            if (scoreDTO.Highscore > current.Score)
            {
                shouldUpload = true;
            }
            else if (scoreDTO.Highscore == current.Score)
            {
                if (scoreDTO.IsCleared && !current.IsCleared)
                    shouldUpload = true;
                else if (scoreDTO.IsCleared == current.IsCleared &&
                         scoreDTO.ElapsedPlayTime < current.ElapsedPlayTime)
                    shouldUpload = true;
            }
        }

        if (shouldUpload)
        {
            var newEntry = new RankingEntry(scoreDTO);
            await docRef.SetAsync(newEntry);
            Debug.Log("내 점수를 Firestore에 업로드함");
        }
    }
}
