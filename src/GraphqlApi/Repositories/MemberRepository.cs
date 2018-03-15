using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLApi.Interfaces;
using GraphQLApi.Models;
using GraphQLApi.Schema.Types;

namespace GraphQLApi.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private static readonly ConcurrentDictionary<string, List<MemberDto>> _members = new ConcurrentDictionary<string, List<MemberDto>>();
        public Task<List<MemberDto>> GetMembersForTeam(string id) =>
            Task.FromResult(_members.ContainsKey(id) ? _members.GetValueOrDefault(id) : new List<MemberDto>());

        public Task<MemberDto> AddMember(string teamId, MemberDto member)
        {
            member.Id = Guid.NewGuid().ToString();
            _members.AddOrUpdate(teamId, new List<MemberDto> { member }, (id, storedMembers) =>
            {
                storedMembers.Add(member);
                return storedMembers;
            });
            return Task.FromResult(member);
        }
    }
}
