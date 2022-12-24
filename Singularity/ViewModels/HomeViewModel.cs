using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Core.Contracts.Services;
using Singularity.Core.Helpers;
using Singularity.Models;
using Singularity.Core.Services;

namespace Singularity.ViewModels;

public partial class HomeViewModel : ObservableRecipient
{
    [ObservableProperty]
    public ObservableCollection<Genre>? genres;
    public HomeViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;
        PopulateFeed();
    }
    void PopulateFeed()
    {
        var genres = new ObservableCollection<Genre>
        {
            new("Chill", "RDCLAK5uy_m7I7OhxMQp4dAK2AKvrEoiNmrIDnAX5Z8",
            "https://lh3.googleusercontent.com/phet14I_8rdgCq-g6JPi8MoQQU5ZOnIzrDoTWtdhlVkz1wlCe-drqdkb2Qfxggd6C66g1wejgx9uxd1Y=w544-h544-l90-rj"),
            new("Focus", "RDCLAK5uy_kb7EBi6y3GrtJri4_ZH56Ms786DFEimbM",
            "https://lh3.googleusercontent.com/Aen64Gcp92yGdBM6LLWFk8vXDF3l6fzWHnyRJJFmjOcqZovwI9ddtd0ONGGaLUYRiRCswTDhcY8DFM1h=w544-h544-l90-rj"),
            new("Workout","RDCLAK5uy_k1VVBVsS6pu1pVkYZK2B0EWic3i4j_TY4",
            "https://lh3.googleusercontent.com/1F7jqgt5lmSgNweG3MYXmYw4yv9xYle4WH_XQ-o72iuHlxI8MLZV_g7AtnofyBQVocQpi6VWhbFCVSE=w544-h544-l90-rj"),
            new("Party","RDCLAK5uy_mdFeaJL-S0sClXygtjtCC1WlP1F0-qRRk",
            "https://lh3.googleusercontent.com/Ec9oaPzQ3TafPzGzlE-XtfzTsuXo096vRc80LyS_mwODj9VsDOT99ZUyJonFX5momoir9Ri212F0qLMc=w544-h544-l90-rj"),
            new("Romance","RDCLAK5uy_l1oO11DBO4FD8U7bOrqUKK5Y_PkISUMQM",
            "https://lh3.googleusercontent.com/PgqPJM_3mqB-QZSMPCfMvRKmOYMnNFZtzW-Y6UqT4V9va_nYKC24Y5OGhUAWaaM78eTMlIAWbHbcdQuB=w544-h544-l90-rj"),
            new("Sleep","RDCLAK5uy_lakC34Al6Kd5kidN8Bq0jpdnGUpIw2ctQ",
            "https://lh3.googleusercontent.com/wIfBUg3xNQfRL-P-x_XfS_JRLwczofydHbXpWSvojY8zknDeh0Qtn-A4TcMGdimbJsZBabXj8hklugU=w544-h544-l90-rj"),
            new("Pop","RDCLAK5uy_kmPRjHDECIcuVwnKsx2Ng7fyNgFKWNJFs",
            "https://lh3.googleusercontent.com/IKHjQxfVtM30cCqti1smV5AkTwd4uSrwO0E5pEfIwxzniqDfmL5RXuYz7psnCUk62vdN4zRkhwQLhFI=w544-h544-l90-rj"),
            new("Hi hop","RDCLAK5uy_mVRuj5egfh21e-pXyA3ymx_0p4Xlg-c0I",
            "https://lh3.googleusercontent.com/JSNZvTvnJWSlcwtboCS7vBTK9uPxg6UEkX6v-MNBlDo5P6ShvAYnuzGvPdTem22d5-hsM_5WYTn2ng=w544-h544-l90-rj"),
            new("Energetic","RDCLAK5uy_mVJ3RRi_YBfUJnZnQxLAedQQcXHujbUcg",
            "https://lh3.googleusercontent.com/jzUe3KUHUMCHGN5o02havVoL481hvjMJ1Yjgyej8xPYwCfGwPKrPlcUmBr0p9HjbxSSzR8ZvzsVsuY3r=w544-h544-l90-rj"),
            new("Feel Good","RDCLAK5uy_kJWGcrtTC_zrbD6rKkBvOcht_vzijhX1A",
            "https://lh3.googleusercontent.com/45AIiZAuFBHr_Zn--AyJziC-8VSaHbOeZAZarPF0AOpmQLGnLVMWsEADvpJr9LEYetpSCr-5sNairqM=w544-h544-l90-rj"),
            new("Rock","RDCLAK5uy_nLtxizvEMkzYQUrA-bFf6MnBeR4bGYWUQ",
            "https://lh3.googleusercontent.com/Z4GAr1MIJGFoGHu7j4WBcj1UNYtLGUhRdKETlMukzb8cW8sbj7F7gdNdd3YLh6J5Aa3qTQ1cSEsiFA=w544-h544-l90-rj"),
            new("Metal","RDCLAK5uy_mk6AmqcHgCRhyJuYsQz5CCVdCF4SRGivs",
            "https://lh3.googleusercontent.com/DgFLQxlb1AgKVX_whbwQcHae78nK76r-yM21iWP6_6TgaWHkbweezed35_7QeqJQ266ybUXwnskJ0A=w544-h544-l90-rj"),
            new("Jazz","RDCLAK5uy_nm4NA8cldZNPqj1D0ayZxfySeY89qedRs",
            "https://lh3.googleusercontent.com/_fJbAPvRvDFktq7Ot97d-FPGQW3zkOpcxgb-B369Tq9h7fVm5LpUWJoea-fuZ0FbWQFdTfhSzqKmOnI=w544-h544-l90-rj"),
            new("Bollywood","RDCLAK5uy_l8ohbe556smHVTBGt3YcKCsrablXt_BXk",
            "https://lh3.googleusercontent.com/CaPfOp6RoWDMzh5fDlMRpli-_oiCa5U4ekHPcVUAD14p0jRQ5f78EFbPBHhQL2ityafwnbigEJWoSKc=w544-h544-l90-rj"),
            new("Folk","RDCLAK5uy_kNt1WT5NSpZ4HErcqC6sDbZlPTpsZW9aM",
            "https://lh3.googleusercontent.com/VNZ-8PPGDKOOa9jfHRyFd9j6wl75UWfqqCblKq5y4jnHHY9CWPkRAVXrfXSz6Bb75kmGo59Whp8VcGc=w544-h544-l90-rj"),
            new("Sufi","RDCLAK5uy_ngT3H4Vu-YMwwjFPt6Ocr3n7j2l-cUAeQ",
            "https://lh3.googleusercontent.com/akan1kC6j4dT2L41_IJPwkVSlHAscgZ5mq2qZCTI44K3hIi6VzVud_2G9TSBx1sxeTHOzOEp57-QZg=w544-h544-l90-rj"),
            new("Children","RDCLAK5uy_kIdiH02YL21o6iOEuDqTNq6bFpzBBTjL8",
            "https://lh3.googleusercontent.com/ey1dHgi_P4yiBqvHkltAJeDfFBVYzjfWg89dujxN4NFYgGhk8WWCTfmYtFJhG10pdRtTCcIFVEs7R3Q=w544-h544-l90-rj"),
            new("Electronic","RDCLAK5uy_kLWIr9gv1XLlPbaDS965-Db4TrBoUTxQ8",
            "https://lh3.googleusercontent.com/G47M7e2kx49FlveyDwR8ICg9Expl6KVGEvt3CbcMRyaMupu6fatNDDfDHJIjGyoa0iUvJ1vp2RKDvH8=w544-h544-l90-rj"),
            new("Contry","RDCLAK5uy_lJ8xZWiZj2GCw7MArjakb6b0zfvqwldps",
            "https://lh3.googleusercontent.com/vi9PNU0niJ9EP5QVkxHSqdP-j8p70YEaAFcaD6JG3DDCAUsbUdR6DipW-AilecFmQftlwa83WpOxXeQ=w544-h544-l90-rj"),
            new("Monsoon","RDCLAK5uy_lPzT2bIPNJ_6II2vlgcE_-Mw1fMTfPheA",
            "https://lh3.googleusercontent.com/gaMZdnthGvERNyPgHX7PJPBtOr5svz65hC5vAnShERp8CIFfCO8p515fg2rac7qiojccJYwI9GSdNWA=w544-h544-l90-rj"),
        };

        Genres = genres;
    }

    public IYoutubeService Youtube
    {
        get;
    }

}
