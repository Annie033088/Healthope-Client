using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IMemberRepository
    {
        (int errorCodeNum, int memberId) AddMember(AddMemberDto addMemberDto);
    }
}
