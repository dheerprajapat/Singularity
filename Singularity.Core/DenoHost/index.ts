import express from "npm:express@4.18.2";
// @deno-types="./browser.d.ts"
import { Innertube } from './browser.js';

const app = express();


app.get("/", (_, res) => {
  res.send("Welcome to the Dinosaur API!");
});

app.get("/ping", (_, res) => {
    res.send("up");
});

app.get('/feed',async(_,res) =>
{
  res.send(JSON.stringify(await yt.music.getHomeFeed()  ))
});

app.listen(9867);


