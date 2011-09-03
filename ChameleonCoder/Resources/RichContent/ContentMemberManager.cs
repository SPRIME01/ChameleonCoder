﻿using System;

namespace ChameleonCoder.Resources.RichContent
{
    internal static class ContentMemberManager
    {
        private static ContentMemberCollection ContentMembers = new ContentMemberCollection();

        internal static void RegisterComponent(Type component, ContentMemberTypeInfo info)
        {
            if (component.GetInterface(typeof(IContentMember).FullName) != null
                && !component.IsAbstract && !component.IsInterface && !component.IsNotPublic
                && !ContentMembers.IsRegistered(info.Alias))
            {
                ContentMembers.RegisterMember(info, component);
            }
        }

        internal static IContentMember CreateInstanceOf(string alias)
        {
            Type member = ContentMembers.GetMember(alias);
            if (member == null)
                return null;
            return Activator.CreateInstance(member) as IContentMember;
        }
    }
}
