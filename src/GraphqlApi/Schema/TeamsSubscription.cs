using System.Threading.Tasks;
using GraphQLApi.Interfaces;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;
using GraphQLApi.Schema.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using GraphQLApi.Models;

namespace GraphQLApi.Schema
{
    [ImplementViewer(OperationType.Subscription)]
    public class TeamsSubscription
    {
        private readonly IMemberRepository _memberRepository;

        public TeamsSubscription(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }
    }
}
