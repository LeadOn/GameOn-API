// <copyright file="HighlightBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business
{
    using YuGames.Business.Contracts;
    using YuGames.Entities;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// Highlight business.
    /// </summary>
    public class HighlightBusiness : IHighlightBusiness
    {
        private IHighlightRepository highlightRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightBusiness" /> class.
        /// </summary>
        /// <param name="highlightRepository">Highlight repository, injected.</param>
        public HighlightBusiness(IHighlightRepository highlightRepository)
        {
            this.highlightRepository = highlightRepository;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Highlight>> GetAll()
        {
            return await this.highlightRepository.GetAll();
        }
    }
}