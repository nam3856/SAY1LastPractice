using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRankingRepository
{
    Task SaveAsync(List<RankingEntry> rankingList);
    Task<List<RankingEntry>> LoadAsync();
}