

using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IMemberRepository
{
    void Update(Member member);
    Task<PaginatedResut<Member>> GetMembersAsync(MemberParams memberParams);
    Task<Member?> GetMemberByIDAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotoForMemberAsync(string memberID);
    Task<Member?> GetMemberForUpdate(string id);
}