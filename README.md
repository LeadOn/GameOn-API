<p align="center">
    <img src="./images/gameon-logo.png" width="128" />
</p>

<h1 align="center">GameOn! API</h1>

# Description

This project is a back-end for the GameOn! project, done using ASP.NET on the .NET 9 framework, and will be running live on [this site](https://gameon-api.valentinvirot.fr/).

First, the project was built around the N-Layer architecture (you can find it in the archive/n-layer branch).
Now, the project is using the Clean Architecture + CQRS design patterns, that suits better in this use case.

## How to run it?

You can use the associated Dockerfile to build an image, or directly build it.
Here are all of the environment variables currently used :

<ul>
    <li>DB_CONNECTION_STRING: MySQL connexion string.</li>
    <li>JWT_AUTHORITY: Token authority (used in the authentication middleware).</li>
    <li>JWT_AUDIENCE: Token audience (used in the authentication middleware).</li>
    <li>CURRENT_SEASON: Current season ID (used for FIFA/Soccer stats management).</li>
    <li>DEFAULT_PLATFORM: Default platform to be used (used for FIFA/Soccer stats management).</li>
    <li>RIOT_GAMES_ACCOUNT_API_ROUTE: Riot Games API route to be used for Account management (exemple: europe.api.riotgames.com).</li>
    <li>RIOT_GAMES_SUMMONER_API_ROUTE: Riot Games API route to be used for Summoner management (exemple: euw1.api.riotgames.com).</li>
    <li>RIOT_GAMES_API_KEY: Riot Games API Key (can be obtained via Riot's official developer website).</li>
    <li>MEDIATR_LICENSE_KEY: MediatR License Key (can be obtained via their official website).</li>
    <li>S3_ENDPOINT: Endpoint to S3 Bucket.</li>
    <li>S3_ACCESS_KEY: S3 Bucket Access key.</li>
    <li>S3_SECRET_KEY: S3 Bucket Secret key.</li>
    <li>S3_BUCKET_NAME: S3 Bucket Name.</li>
    <li>S3_PP_BASE_PATH: Base path to profile pictures in S3 bucket.</li>
    <li>S3_TP_BASE_PATH: Base path to tournament pictures in S3 bucket.</li>
    <li>DEFAULT_PROFILE_PIC: Default profile picture name.</li>
</ul>

## Can I use it for my own?

If this repo is public, you can clone/fork my project and use it for your own website. Just credit me somewhere in your site, as a thanks for my work :)

You can contact me using my [Portfolio](https://www.valentinvirot.fr), if you have some question about it. Have fun!
