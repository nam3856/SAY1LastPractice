using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirestoreRankingRepository : IRankingRepository
{
    private FirebaseFirestore _db;
    private const string COLLECTION_NAME = "rankings";

    public FirestoreRankingRepository()
    {
        _db = FirebaseFirestore.DefaultInstance;
    }

    public async Task SaveAsync(List<RankingEntry> rankingList)
    {
        // rankings 컬렉션을 전체 덮어쓰기
        var batch = _db.StartBatch();

        for (int i = 0; i < rankingList.Count; i++)
        {
            var docRef = _db.Collection(COLLECTION_NAME).Document(rankingList[i].PlayerId);
            batch.Set(docRef, rankingList[i]);
        }

        await batch.CommitAsync();
        Debug.Log("랭킹 Firestore 저장 완료");
    }

    public async Task<List<RankingEntry>> LoadAsync()
    {
        var snapshot = await _db.Collection(COLLECTION_NAME).GetSnapshotAsync();

        List<RankingEntry> list = new List<RankingEntry>();
        foreach (var doc in snapshot.Documents)
        {
            var entry = doc.ConvertTo<RankingEntry>();
            list.Add(entry);
        }

        return list;
    }
}
