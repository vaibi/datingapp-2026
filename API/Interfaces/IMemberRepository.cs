

using API.Entities;

namespace API.Interfaces;

public interface IMemberRepository
{
    void Update(Member member);
    Task<bool> SaveAllAsync();
    Task<IReadOnlyList<Member>> GetMembersAsync();
    Task<Member?> GetMemberByIDAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotoForMemberAsync(string memberID);
    Task<Member?> GetMemberForUpdate(string id);
}