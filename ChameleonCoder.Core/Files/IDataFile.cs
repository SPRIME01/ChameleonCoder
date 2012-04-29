﻿using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.Files
{
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("5DAC5D33-6E2F-4C05-B400-14340196B875")]
    public interface IDataFile
    {
        void Initialize(ChameleonCoderApp app);
        void Shutdown();
        bool IsInitialized { get; }

        void Open(string path);
        bool IsOpened { get; }

        void Save();

        [Obsolete]
        bool IsLoaded { get; }
        [Obsolete]
        void Load();

        void SetMetadata(string key, string value);
        string GetMetadata(string key); // todo: make observable
        StringDictionary GetMetadata();
        void DeleteMetadata(string key);

        void AddFileReference(string path);
        void AddDirectoryReference(string path);
        bool HasFileReference(string path);
        bool HasDirectoryReference(string path);
        string[] FileReferences { get; }
        string[] DirectoryReferences { get; }
        void DeleteFileReference(string path);
        void DeleteDirectoryReference(string path);

        void ResourceDelete(IResource resource);
        void ResourceInsert(IResource resource, IResource parent);

        void ResourceSetCreatedDate(IResource resource);
        void ResourceSetCreatedDate(IResource resource, DateTime time);
        DateTime ResourceGetCreatedDate(IResource resource);

        void ResourceUpdateLastModified(IResource resource);
        void ResourceUpdateLastModified(IResource resource, DateTime time);
        DateTime ResourceGetLastModified(IResource resource);

        void ResourceSetMetadata(IResource resource, string key, string value);
        string ResourceGetMetadata(IResource resource, string key);
        StringDictionary ResourceGetMetadata(IResource resource); // todo: make observable
        void ResourceDeleteMetadata(IResource resource, string key);

        ObservableStringDictionary[] ResourceGetRichContent(IRichContentResource resource);
        ObservableStringDictionary[] ContentMemberGetChildren(IContentMember member);

        string FilePath { get; }
        string Name { get; }
        ChameleonCoderApp App { get; }
    }
}
