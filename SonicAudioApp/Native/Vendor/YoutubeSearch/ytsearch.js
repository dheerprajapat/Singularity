const yts=require('yt-search');
//use await in anonyomous function
 ((async () => {
      const query=process.argv[2];
      //console.log(process.argv);
      const amount=parseInt(process.argv[3]);

      const r =await  yts(query);
    
      const videos = r.videos.slice(0,amount);
      console.log(JSON.stringify(videos));
 }))();

 /*
 Compiled to native exe by using Nexe
 `nexe ytsearch.js --build --verbose -t windows` with windows-build tools and NASM 
 */