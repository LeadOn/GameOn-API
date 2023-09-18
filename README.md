<h1 align="center">YuGames API</h1>

# Description

This project is a back-end for the YuGames project, done using ASP.NET on the .NET 7 framework, and will be running live on [this site](https://yugames-api.valentinvirot.fr/).

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

## Can I use it for my own website?

If this repo is public, you can clone/fork my project and use it for your own website. Just credit me somewhere in your site / code, as a thanks for my work :)

You can contact me using my [Twitter](https://twitter.com/valentin_vir) account, if you have some question about it. Have fun!
