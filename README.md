<p align="center">
    <img src="./images/gameon-logo.png" width="128" />
</p>

<h1 align="center">GameOn! API</h1>

# Description

This project is a back-end for the GameOn! project, done using ASP.NET on the .NET 8 framework, and will be running live on [this site](https://gameon-api.valentinvirot.fr/).

First, the project was built around the N-Layer architecture (you can find it in the archive/n-layer branch).
Now, the project is using the Clean Architecture + CQRS design patterns, that suits better in this use case.

## How to run it?

You can use the associated Dockerfile to build an image, or directly build it.
Here are all of the environment variables currently used :

<ul>
    <li>SQLITE_PATH: Path to the SQLite database file.</li>
    <li>JWT_AUTHORITY: Token authority (used in the authentication middleware).</li>
    <li>JWT_AUDIENCE: Token audience (used in the authentication middleware).</li>
    <li>CURRENT_SEASON: Current season ID in database.</li>
    <li>DEFAULT_PLATFORM: Default platform to be used.</li>
</ul>

## Can I use it for my own?

If this repo is public, you can clone/fork my project and use it for your own website. Just credit me somewhere in your site, as a thanks for my work :)

You can contact me using my [Portfolio](https://www.valentinvirot.fr), if you have some question about it. Have fun!
