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
        private IPlayerBusiness playerBusiness;
        private IFifaGamePlayedBusiness fifaGamePlayedBusiness;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightBusiness" /> class.
        /// </summary>
        /// <param name="highlightRepository">Highlight repository, injected.</param>
        /// <param name="playerBusiness">Player business, injected.</param>
        /// <param name="fifaGamePlayedBusiness">FifaGamePlayed repository, injected.</param>
        public HighlightBusiness(IHighlightRepository highlightRepository, IPlayerBusiness playerBusiness, IFifaGamePlayedBusiness fifaGamePlayedBusiness)
        {
            this.highlightRepository = highlightRepository;
            this.playerBusiness = playerBusiness;
            this.fifaGamePlayedBusiness = fifaGamePlayedBusiness;
        }

        /// <inheritdoc />
        public async Task<Highlight> Create(string name, string? description, int playerId, int fifaGameId, string? externalUrl)
        {
            var playerInDb = await this.playerBusiness.GetPlayerById(playerId);

            if (playerInDb is null)
            {
                throw new NotImplementedException();
            }
            else
            {
                var gameInDb = await this.fifaGamePlayedBusiness.GetById(fifaGameId);

                if (gameInDb is null)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    var highlight = new Highlight
                    {
                        CreatedById = playerId,
                        Name = name,
                        Description = description,
                        ExternalUrl = externalUrl,
                        FifaGameId = fifaGameId,
                    };

                    return await this.highlightRepository.Create(highlight);
                }
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Highlight>> GetAll()
        {
            return await this.highlightRepository.GetAll();
        }
    }
}