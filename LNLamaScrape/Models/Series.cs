﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LNLamaScrape.Models;
using LNLamaScrape.Repository;

namespace LNLamaScrape.Models
{
    public class Series : ISeries
    {
        readonly RepositoryBase _parentRepositoryInternal;
        internal IRepository ParentRepository => _parentRepositoryInternal;
        public string Title { get; private set; }

        public string Description { get; internal set; }
        public string[] TitlesAlternative { get; internal set; }
        public Uri SeriesPageUri { get; internal set; }
        public Uri CoverImageUri { get; internal set; }
        public string Author { get; internal set; }
        public string Updated { get; internal set; }
        public string[] Tags { get; internal set; }
        public string[] Genres { get; internal set; }

        public Series(RepositoryBase parent, Uri seriesPageUri, string title)
        {
            _parentRepositoryInternal = parent;
            SeriesPageUri = seriesPageUri;
            Title = title;
            Updated = string.Empty;
        }

        public IRepository GetParentRepository()
        {
            return ParentRepository;
        }
        public Task<byte[]> GetCoverAsync()
        {
            using (var cts = new CancellationTokenSource())
            {
                return GetCoverAsync(cts.Token);
            }
        }
        public Task<byte[]> GetCoverAsync(CancellationToken token)
        {
            return _parentRepositoryInternal.GetCoverImageAsync(this, token);
        }

        public Task<IReadOnlyList<IChapter>> GetChaptersAsync()
        {
            using (var cts = new CancellationTokenSource())
            {
                return GetChaptersAsync(cts.Token);
            }
        }

        public virtual Task<IReadOnlyList<IChapter>> GetChaptersAsync(CancellationToken token)
        {
            return _parentRepositoryInternal.GetChaptersAsync(this, token);
        }

        public void UpdateSeriesDetails(string description = null, string[] titlesAlternative = null, Uri coverImageUri = null, string author = null, string updated = null, string[] tags = null, string[] genres = null)
        { 
            if (!string.IsNullOrWhiteSpace(description))
                Description = description;
            if (titlesAlternative != null)
                TitlesAlternative = titlesAlternative;
            if (coverImageUri != null)
                CoverImageUri = coverImageUri;
            if (!string.IsNullOrWhiteSpace(author))
                Author = author;
            if (!string.IsNullOrWhiteSpace(updated))
                Updated = updated;
            if (tags != null)
                Tags = tags;
            if (genres != null)
                Genres = genres;
        }
    }
}
