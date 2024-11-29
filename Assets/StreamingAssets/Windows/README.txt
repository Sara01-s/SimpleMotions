FFmpeg 64-bit static Windows build from www.gyan.dev

Version: 2024-11-28-git-bc991ca048-essentials_build-www.gyan.dev

License: GPL v3

Source Code: https://github.com/FFmpeg/FFmpeg/commit/bc991ca048

git-essentials build configuration: 

ARCH                      x86 (generic)
big-endian                no
runtime cpu detection     yes
standalone assembly       yes
x86 assembler             nasm
MMX enabled               yes
MMXEXT enabled            yes
3DNow! enabled            yes
3DNow! extended enabled   yes
SSE enabled               yes
SSSE3 enabled             yes
AESNI enabled             yes
AVX enabled               yes
AVX2 enabled              yes
AVX-512 enabled           yes
AVX-512ICL enabled        yes
XOP enabled               yes
FMA3 enabled              yes
FMA4 enabled              yes
i686 features enabled     yes
CMOV is fast              yes
EBX available             yes
EBP available             yes
debug symbols             yes
strip symbols             yes
optimize for size         no
optimizations             yes
static                    yes
shared                    no
postprocessing support    yes
network support           yes
threading support         pthreads
safe bitstream reader     yes
texi2html enabled         no
perl enabled              yes
pod2man enabled           yes
makeinfo enabled          yes
makeinfo supports HTML    yes
xmllint enabled           yes

External libraries:
avisynth                libopencore_amrnb       libvpx
bzlib                   libopencore_amrwb       libwebp
gmp                     libopenjpeg             libx264
gnutls                  libopenmpt              libx265
iconv                   libopus                 libxml2
libaom                  librubberband           libxvid
libass                  libspeex                libzimg
libfontconfig           libsrt                  libzmq
libfreetype             libssh                  lzma
libfribidi              libtheora               mediafoundation
libgme                  libvidstab              sdl2
libgsm                  libvmaf                 zlib
libharfbuzz             libvo_amrwbenc
libmp3lame              libvorbis

External libraries providing hardware acceleration:
amf                     d3d12va                 nvdec
cuda                    dxva2                   nvenc
cuda_llvm               ffnvcodec               vaapi
cuvid                   libmfx
d3d11va                 libvpl

Libraries:
avcodec                 avformat                swresample
avdevice                avutil                  swscale
avfilter                postproc

Programs:
ffmpeg                  ffplay                  ffprobe

Enabled decoders:
aac                     frwu                    pgssub
aac_fixed               ftr                     pgx
aac_latm                g2m                     phm
aasc                    g723_1                  photocd
ac3                     g729                    pictor
ac3_fixed               gdv                     pixlet
acelp_kelvin            gem                     pjs
adpcm_4xm               gif                     png
adpcm_adx               gremlin_dpcm            ppm
adpcm_afc               gsm                     prores
adpcm_agm               gsm_ms                  prosumer
adpcm_aica              h261                    psd
adpcm_argo              h263                    ptx
adpcm_ct                h263i                   qcelp
adpcm_dtk               h263p                   qdm2
adpcm_ea                h264                    qdmc
adpcm_ea_maxis_xa       h264_cuvid              qdraw
adpcm_ea_r1             h264_qsv                qoa
adpcm_ea_r2             hap                     qoi
adpcm_ea_r3             hca                     qpeg
adpcm_ea_xas            hcom                    qtrle
adpcm_g722              hdr                     r10k
adpcm_g726              hevc                    r210
adpcm_g726le            hevc_cuvid              ra_144
adpcm_ima_acorn         hevc_qsv                ra_288
adpcm_ima_alp           hnm4_video              ralf
adpcm_ima_amv           hq_hqa                  rasc
adpcm_ima_apc           hqx                     rawvideo
adpcm_ima_apm           huffyuv                 realtext
adpcm_ima_cunning       hymt                    rka
adpcm_ima_dat4          iac                     rl2
adpcm_ima_dk3           idcin                   roq
adpcm_ima_dk4           idf                     roq_dpcm
adpcm_ima_ea_eacs       iff_ilbm                rpza
adpcm_ima_ea_sead       ilbc                    rscc
adpcm_ima_iss           imc                     rtv1
adpcm_ima_moflex        imm4                    rv10
adpcm_ima_mtf           imm5                    rv20
adpcm_ima_oki           indeo2                  rv30
adpcm_ima_qt            indeo3                  rv40
adpcm_ima_rad           indeo4                  rv60
adpcm_ima_smjpeg        indeo5                  s302m
adpcm_ima_ssi           interplay_acm           sami
adpcm_ima_wav           interplay_dpcm          sanm
adpcm_ima_ws            interplay_video         sbc
adpcm_ms                ipu                     scpr
adpcm_mtaf              jacosub                 screenpresso
adpcm_psx               jpeg2000                sdx2_dpcm
adpcm_sbpro_2           jpegls                  sga
adpcm_sbpro_3           jv                      sgi
adpcm_sbpro_4           kgv1                    sgirle
adpcm_swf               kmvc                    sheervideo
adpcm_thp               lagarith                shorten
adpcm_thp_le            lead                    simbiosis_imx
adpcm_vima              libaom_av1              sipr
adpcm_xa                libgsm                  siren
adpcm_xmd               libgsm_ms               smackaud
adpcm_yamaha            libopencore_amrnb       smacker
adpcm_zork              libopencore_amrwb       smc
agm                     libopus                 smvjpeg
aic                     libspeex                snow
alac                    libvorbis               sol_dpcm
alias_pix               libvpx_vp8              sonic
als                     libvpx_vp9              sp5x
amrnb                   loco                    speedhq
amrwb                   lscr                    speex
amv                     m101                    srgc
anm                     mace3                   srt
ansi                    mace6                   ssa
anull                   magicyuv                stl
apac                    mdec                    subrip
ape                     media100                subviewer
apng                    metasound               subviewer1
aptx                    microdvd                sunrast
aptx_hd                 mimic                   svq1
arbc                    misc4                   svq3
argo                    mjpeg                   tak
ass                     mjpeg_cuvid             targa
asv1                    mjpeg_qsv               targa_y216
asv2                    mjpegb                  tdsc
atrac1                  mlp                     text
atrac3                  mmvideo                 theora
atrac3al                mobiclip                thp
atrac3p                 motionpixels            tiertexseqvideo
atrac3pal               movtext                 tiff
atrac9                  mp1                     tmv
aura                    mp1float                truehd
aura2                   mp2                     truemotion1
av1                     mp2float                truemotion2
av1_cuvid               mp3                     truemotion2rt
av1_qsv                 mp3adu                  truespeech
avrn                    mp3adufloat             tscc
avrp                    mp3float                tscc2
avs                     mp3on4                  tta
avui                    mp3on4float             twinvq
bethsoftvid             mpc7                    txd
bfi                     mpc8                    ulti
bink                    mpeg1_cuvid             utvideo
binkaudio_dct           mpeg1video              v210
binkaudio_rdft          mpeg2_cuvid             v210x
bintext                 mpeg2_qsv               v308
bitpacked               mpeg2video              v408
bmp                     mpeg4                   v410
bmv_audio               mpeg4_cuvid             vb
bmv_video               mpegvideo               vble
bonk                    mpl2                    vbn
brender_pix             msa1                    vc1
c93                     mscc                    vc1_cuvid
cavs                    msmpeg4v1               vc1_qsv
cbd2_dpcm               msmpeg4v2               vc1image
ccaption                msmpeg4v3               vcr1
cdgraphics              msnsiren                vmdaudio
cdtoons                 msp2                    vmdvideo
cdxl                    msrle                   vmix
cfhd                    mss1                    vmnc
cinepak                 mss2                    vnull
clearvideo              msvideo1                vorbis
cljr                    mszh                    vp3
cllc                    mts2                    vp4
comfortnoise            mv30                    vp5
cook                    mvc1                    vp6
cpia                    mvc2                    vp6a
cri                     mvdv                    vp6f
cscd                    mvha                    vp7
cyuv                    mwsc                    vp8
dca                     mxpeg                   vp8_cuvid
dds                     nellymoser              vp8_qsv
derf_dpcm               notchlc                 vp9
dfa                     nuv                     vp9_cuvid
dfpwm                   on2avc                  vp9_qsv
dirac                   opus                    vplayer
dnxhd                   osq                     vqa
dolby_e                 paf_audio               vqc
dpx                     paf_video               vvc
dsd_lsbf                pam                     vvc_qsv
dsd_lsbf_planar         pbm                     wady_dpcm
dsd_msbf                pcm_alaw                wavarc
dsd_msbf_planar         pcm_bluray              wavpack
dsicinaudio             pcm_dvd                 wbmp
dsicinvideo             pcm_f16le               wcmv
dss_sp                  pcm_f24le               webp
dst                     pcm_f32be               webvtt
dvaudio                 pcm_f32le               wmalossless
dvbsub                  pcm_f64be               wmapro
dvdsub                  pcm_f64le               wmav1
dvvideo                 pcm_lxf                 wmav2
dxa                     pcm_mulaw               wmavoice
dxtory                  pcm_s16be               wmv1
dxv                     pcm_s16be_planar        wmv2
eac3                    pcm_s16le               wmv3
eacmv                   pcm_s16le_planar        wmv3image
eamad                   pcm_s24be               wnv1
eatgq                   pcm_s24daud             wrapped_avframe
eatgv                   pcm_s24le               ws_snd1
eatqi                   pcm_s24le_planar        xan_dpcm
eightbps                pcm_s32be               xan_wc3
eightsvx_exp            pcm_s32le               xan_wc4
eightsvx_fib            pcm_s32le_planar        xbin
escape124               pcm_s64be               xbm
escape130               pcm_s64le               xface
evrc                    pcm_s8                  xl
exr                     pcm_s8_planar           xma1
fastaudio               pcm_sga                 xma2
ffv1                    pcm_u16be               xpm
ffvhuff                 pcm_u16le               xsub
ffwavesynth             pcm_u24be               xwd
fic                     pcm_u24le               y41p
fits                    pcm_u32be               ylc
flac                    pcm_u32le               yop
flashsv                 pcm_u8                  yuv4
flashsv2                pcm_vidc                zero12v
flic                    pcx                     zerocodec
flv                     pdv                     zlib
fmvc                    pfm                     zmbv
fourxm                  pgm
fraps                   pgmyuv

Enabled encoders:
a64multi                hevc_d3d12va            pcm_u16le
a64multi5               hevc_mf                 pcm_u24be
aac                     hevc_nvenc              pcm_u24le
aac_mf                  hevc_qsv                pcm_u32be
ac3                     hevc_vaapi              pcm_u32le
ac3_fixed               huffyuv                 pcm_u8
ac3_mf                  jpeg2000                pcm_vidc
adpcm_adx               jpegls                  pcx
adpcm_argo              libaom_av1              pfm
adpcm_g722              libgsm                  pgm
adpcm_g726              libgsm_ms               pgmyuv
adpcm_g726le            libmp3lame              phm
adpcm_ima_alp           libopencore_amrnb       png
adpcm_ima_amv           libopenjpeg             ppm
adpcm_ima_apm           libopus                 prores
adpcm_ima_qt            libspeex                prores_aw
adpcm_ima_ssi           libtheora               prores_ks
adpcm_ima_wav           libvo_amrwbenc          qoi
adpcm_ima_ws            libvorbis               qtrle
adpcm_ms                libvpx_vp8              r10k
adpcm_swf               libvpx_vp9              r210
adpcm_yamaha            libwebp                 ra_144
alac                    libwebp_anim            rawvideo
alias_pix               libx264                 roq
amv                     libx264rgb              roq_dpcm
anull                   libx265                 rpza
apng                    libxvid                 rv10
aptx                    ljpeg                   rv20
aptx_hd                 magicyuv                s302m
ass                     mjpeg                   sbc
asv1                    mjpeg_qsv               sgi
asv2                    mjpeg_vaapi             smc
av1_amf                 mlp                     snow
av1_mf                  movtext                 speedhq
av1_nvenc               mp2                     srt
av1_qsv                 mp2fixed                ssa
av1_vaapi               mp3_mf                  subrip
avrp                    mpeg1video              sunrast
avui                    mpeg2_qsv               svq1
bitpacked               mpeg2_vaapi             targa
bmp                     mpeg2video              text
cfhd                    mpeg4                   tiff
cinepak                 msmpeg4v2               truehd
cljr                    msmpeg4v3               tta
comfortnoise            msrle                   ttml
dca                     msvideo1                utvideo
dfpwm                   nellymoser              v210
dnxhd                   opus                    v308
dpx                     pam                     v408
dvbsub                  pbm                     v410
dvdsub                  pcm_alaw                vbn
dvvideo                 pcm_bluray              vc2
dxv                     pcm_dvd                 vnull
eac3                    pcm_f32be               vorbis
exr                     pcm_f32le               vp8_vaapi
ffv1                    pcm_f64be               vp9_qsv
ffvhuff                 pcm_f64le               vp9_vaapi
fits                    pcm_mulaw               wavpack
flac                    pcm_s16be               wbmp
flashsv                 pcm_s16be_planar        webvtt
flashsv2                pcm_s16le               wmav1
flv                     pcm_s16le_planar        wmav2
g723_1                  pcm_s24be               wmv1
gif                     pcm_s24daud             wmv2
h261                    pcm_s24le               wrapped_avframe
h263                    pcm_s24le_planar        xbm
h263p                   pcm_s32be               xface
h264_amf                pcm_s32le               xsub
h264_mf                 pcm_s32le_planar        xwd
h264_nvenc              pcm_s64be               y41p
h264_qsv                pcm_s64le               yuv4
h264_vaapi              pcm_s8                  zlib
hdr                     pcm_s8_planar           zmbv
hevc_amf                pcm_u16be

Enabled hwaccels:
av1_d3d11va             hevc_nvdec              vc1_nvdec
av1_d3d11va2            hevc_vaapi              vc1_vaapi
av1_d3d12va             mjpeg_nvdec             vp8_nvdec
av1_dxva2               mjpeg_vaapi             vp8_vaapi
av1_nvdec               mpeg1_nvdec             vp9_d3d11va
av1_vaapi               mpeg2_d3d11va           vp9_d3d11va2
h263_vaapi              mpeg2_d3d11va2          vp9_d3d12va
h264_d3d11va            mpeg2_d3d12va           vp9_dxva2
h264_d3d11va2           mpeg2_dxva2             vp9_nvdec
h264_d3d12va            mpeg2_nvdec             vp9_vaapi
h264_dxva2              mpeg2_vaapi             vvc_vaapi
h264_nvdec              mpeg4_nvdec             wmv3_d3d11va
h264_vaapi              mpeg4_vaapi             wmv3_d3d11va2
hevc_d3d11va            vc1_d3d11va             wmv3_d3d12va
hevc_d3d11va2           vc1_d3d11va2            wmv3_dxva2
hevc_d3d12va            vc1_d3d12va             wmv3_nvdec
hevc_dxva2              vc1_dxva2               wmv3_vaapi

Enabled parsers:
aac                     dvdsub                  mpegvideo
aac_latm                evc                     opus
ac3                     flac                    png
adx                     ftr                     pnm
amr                     g723_1                  qoi
av1                     g729                    rv34
avs2                    gif                     sbc
avs3                    gsm                     sipr
bmp                     h261                    tak
cavsvideo               h263                    vc1
cook                    h264                    vorbis
cri                     hdr                     vp3
dca                     hevc                    vp8
dirac                   ipu                     vp9
dnxhd                   jpeg2000                vvc
dnxuc                   jpegxl                  webp
dolby_e                 misc4                   xbm
dpx                     mjpeg                   xma
dvaudio                 mlp                     xwd
dvbsub                  mpeg4video
dvd_nav                 mpegaudio

Enabled demuxers:
aa                      idf                     pcm_mulaw
aac                     iff                     pcm_s16be
aax                     ifv                     pcm_s16le
ac3                     ilbc                    pcm_s24be
ac4                     image2                  pcm_s24le
ace                     image2_alias_pix        pcm_s32be
acm                     image2_brender_pix      pcm_s32le
act                     image2pipe              pcm_s8
adf                     image_bmp_pipe          pcm_u16be
adp                     image_cri_pipe          pcm_u16le
ads                     image_dds_pipe          pcm_u24be
adx                     image_dpx_pipe          pcm_u24le
aea                     image_exr_pipe          pcm_u32be
afc                     image_gem_pipe          pcm_u32le
aiff                    image_gif_pipe          pcm_u8
aix                     image_hdr_pipe          pcm_vidc
alp                     image_j2k_pipe          pdv
amr                     image_jpeg_pipe         pjs
amrnb                   image_jpegls_pipe       pmp
amrwb                   image_jpegxl_pipe       pp_bnk
anm                     image_pam_pipe          pva
apac                    image_pbm_pipe          pvf
apc                     image_pcx_pipe          qcp
ape                     image_pfm_pipe          qoa
apm                     image_pgm_pipe          r3d
apng                    image_pgmyuv_pipe       rawvideo
aptx                    image_pgx_pipe          rcwt
aptx_hd                 image_phm_pipe          realtext
aqtitle                 image_photocd_pipe      redspark
argo_asf                image_pictor_pipe       rka
argo_brp                image_png_pipe          rl2
argo_cvg                image_ppm_pipe          rm
asf                     image_psd_pipe          roq
asf_o                   image_qdraw_pipe        rpl
ass                     image_qoi_pipe          rsd
ast                     image_sgi_pipe          rso
au                      image_sunrast_pipe      rtp
av1                     image_svg_pipe          rtsp
avi                     image_tiff_pipe         s337m
avisynth                image_vbn_pipe          sami
avr                     image_webp_pipe         sap
avs                     image_xbm_pipe          sbc
avs2                    image_xpm_pipe          sbg
avs3                    image_xwd_pipe          scc
bethsoftvid             imf                     scd
bfi                     ingenient               sdns
bfstm                   ipmovie                 sdp
bink                    ipu                     sdr2
binka                   ircam                   sds
bintext                 iss                     sdx
bit                     iv8                     segafilm
bitpacked               ivf                     ser
bmv                     ivr                     sga
boa                     jacosub                 shorten
bonk                    jpegxl_anim             siff
brstm                   jv                      simbiosis_imx
c93                     kux                     sln
caf                     kvag                    smacker
cavsvideo               laf                     smjpeg
cdg                     lc3                     smush
cdxl                    libgme                  sol
cine                    libopenmpt              sox
codec2                  live_flv                spdif
codec2raw               lmlm4                   srt
concat                  loas                    stl
dash                    lrc                     str
data                    luodat                  subviewer
daud                    lvf                     subviewer1
dcstr                   lxf                     sup
derf                    m4v                     svag
dfa                     matroska                svs
dfpwm                   mca                     swf
dhav                    mcc                     tak
dirac                   mgsts                   tedcaptions
dnxhd                   microdvd                thp
dsf                     mjpeg                   threedostr
dsicin                  mjpeg_2000              tiertexseq
dss                     mlp                     tmv
dts                     mlv                     truehd
dtshd                   mm                      tta
dv                      mmf                     tty
dvbsub                  mods                    txd
dvbtxt                  moflex                  ty
dxa                     mov                     usm
ea                      mp3                     v210
ea_cdata                mpc                     v210x
eac3                    mpc8                    vag
epaf                    mpegps                  vc1
evc                     mpegts                  vc1t
ffmetadata              mpegtsraw               vividas
filmstrip               mpegvideo               vivo
fits                    mpjpeg                  vmd
flac                    mpl2                    vobsub
flic                    mpsub                   voc
flv                     msf                     vpk
fourxm                  msnwc_tcp               vplayer
frm                     msp                     vqf
fsb                     mtaf                    vvc
fwse                    mtv                     w64
g722                    musx                    wady
g723_1                  mv                      wav
g726                    mvi                     wavarc
g726le                  mxf                     wc3
g729                    mxg                     webm_dash_manifest
gdv                     nc                      webvtt
genh                    nistsphere              wsaud
gif                     nsp                     wsd
gsm                     nsv                     wsvqa
gxf                     nut                     wtv
h261                    nuv                     wv
h263                    obu                     wve
h264                    ogg                     xa
hca                     oma                     xbin
hcom                    osq                     xmd
hevc                    paf                     xmv
hls                     pcm_alaw                xvag
hnm                     pcm_f32be               xwma
iamf                    pcm_f32le               yop
ico                     pcm_f64be               yuv4mpegpipe
idcin                   pcm_f64le

Enabled muxers:
a64                     h263                    pcm_s16le
ac3                     h264                    pcm_s24be
ac4                     hash                    pcm_s24le
adts                    hds                     pcm_s32be
adx                     hevc                    pcm_s32le
aea                     hls                     pcm_s8
aiff                    iamf                    pcm_u16be
alp                     ico                     pcm_u16le
amr                     ilbc                    pcm_u24be
amv                     image2                  pcm_u24le
apm                     image2pipe              pcm_u32be
apng                    ipod                    pcm_u32le
aptx                    ircam                   pcm_u8
aptx_hd                 ismv                    pcm_vidc
argo_asf                ivf                     psp
argo_cvg                jacosub                 rawvideo
asf                     kvag                    rcwt
asf_stream              latm                    rm
ass                     lc3                     roq
ast                     lrc                     rso
au                      m4v                     rtp
avi                     matroska                rtp_mpegts
avif                    matroska_audio          rtsp
avm2                    md5                     sap
avs2                    microdvd                sbc
avs3                    mjpeg                   scc
bit                     mkvtimestamp_v2         segafilm
caf                     mlp                     segment
cavsvideo               mmf                     smjpeg
codec2                  mov                     smoothstreaming
codec2raw               mp2                     sox
crc                     mp3                     spdif
dash                    mp4                     spx
data                    mpeg1system             srt
daud                    mpeg1vcd                stream_segment
dfpwm                   mpeg1video              streamhash
dirac                   mpeg2dvd                sup
dnxhd                   mpeg2svcd               swf
dts                     mpeg2video              tee
dv                      mpeg2vob                tg2
eac3                    mpegts                  tgp
evc                     mpjpeg                  truehd
f4v                     mxf                     tta
ffmetadata              mxf_d10                 ttml
fifo                    mxf_opatom              uncodedframecrc
filmstrip               null                    vc1
fits                    nut                     vc1t
flac                    obu                     voc
flv                     oga                     vvc
framecrc                ogg                     w64
framehash               ogv                     wav
framemd5                oma                     webm
g722                    opus                    webm_chunk
g723_1                  pcm_alaw                webm_dash_manifest
g726                    pcm_f32be               webp
g726le                  pcm_f32le               webvtt
gif                     pcm_f64be               wsaud
gsm                     pcm_f64le               wtv
gxf                     pcm_mulaw               wv
h261                    pcm_s16be               yuv4mpegpipe

Enabled protocols:
async                   http                    rtmp
cache                   httpproxy               rtmpe
concat                  https                   rtmps
concatf                 icecast                 rtmpt
crypto                  ipfs_gateway            rtmpte
data                    ipns_gateway            rtmpts
fd                      libsrt                  rtp
ffrtmpcrypt             libssh                  srtp
ffrtmphttp              libzmq                  subfile
file                    md5                     tcp
ftp                     mmsh                    tee
gopher                  mmst                    tls
gophers                 pipe                    udp
hls                     prompeg                 udplite

Enabled filters:
a3dscope                curves                  palettegen
aap                     datascope               paletteuse
abench                  dblur                   pan
abitscope               dcshift                 perlin
acompressor             dctdnoiz                perms
acontrast               ddagrab                 perspective
acopy                   deband                  phase
acrossfade              deblock                 photosensitivity
acrossover              decimate                pixdesctest
acrusher                deconvolve              pixelize
acue                    dedot                   pixscope
addroi                  deesser                 pp
adeclick                deflate                 pp7
adeclip                 deflicker               premultiply
adecorrelate            deinterlace_qsv         prewitt
adelay                  deinterlace_vaapi       procamp_vaapi
adenorm                 dejudder                pseudocolor
aderivative             delogo                  psnr
adrawgraph              denoise_vaapi           pullup
adrc                    deshake                 qp
adynamicequalizer       despill                 random
adynamicsmooth          detelecine              readeia608
aecho                   dialoguenhance          readvitc
aemphasis               dilation                realtime
aeval                   displace                remap
aevalsrc                doubleweave             removegrain
aexciter                drawbox                 removelogo
afade                   drawbox_vaapi           repeatfields
afdelaysrc              drawgraph               replaygain
afftdn                  drawgrid                reverse
afftfilt                drawtext                rgbashift
afir                    drmeter                 rgbtestsrc
afireqsrc               dynaudnorm              roberts
afirsrc                 earwax                  rotate
aformat                 ebur128                 rubberband
afreqshift              edgedetect              sab
afwtdn                  elbg                    scale
agate                   entropy                 scale2ref
agraphmonitor           epx                     scale_cuda
ahistogram              eq                      scale_qsv
aiir                    equalizer               scale_vaapi
aintegral               erosion                 scdet
ainterleave             estdif                  scharr
alatency                exposure                scroll
alimiter                extractplanes           segment
allpass                 extrastereo             select
allrgb                  fade                    selectivecolor
allyuv                  feedback                sendcmd
aloop                   fftdnoiz                separatefields
alphaextract            fftfilt                 setdar
alphamerge              field                   setfield
amerge                  fieldhint               setparams
ametadata               fieldmatch              setpts
amix                    fieldorder              setrange
amovie                  fillborders             setsar
amplify                 find_rect               settb
amultiply               firequalizer            sharpness_vaapi
anequalizer             flanger                 shear
anlmdn                  floodfill               showcqt
anlmf                   format                  showcwt
anlms                   fps                     showfreqs
anoisesrc               framepack               showinfo
anull                   framerate               showpalette
anullsink               framestep               showspatial
anullsrc                freezedetect            showspectrum
apad                    freezeframes            showspectrumpic
aperms                  fspp                    showvolume
aphasemeter             fsync                   showwaves
aphaser                 gblur                   showwavespic
aphaseshift             geq                     shuffleframes
apsnr                   gradfun                 shufflepixels
apsyclip                gradients               shuffleplanes
apulsator               graphmonitor            sidechaincompress
arealtime               grayworld               sidechaingate
aresample               greyedge                sidedata
areverse                guided                  sierpinski
arls                    haas                    signalstats
arnndn                  haldclut                signature
asdr                    haldclutsrc             silencedetect
asegment                hdcd                    silenceremove
aselect                 headphone               sinc
asendcmd                hflip                   sine
asetnsamples            highpass                siti
asetpts                 highshelf               smartblur
asetrate                hilbert                 smptebars
asettb                  histeq                  smptehdbars
ashowinfo               histogram               sobel
asidedata               hqdn3d                  spectrumsynth
asisdr                  hqx                     speechnorm
asoftclip               hstack                  split
aspectralstats          hstack_qsv              spp
asplit                  hstack_vaapi            ssim
ass                     hsvhold                 ssim360
astats                  hsvkey                  stereo3d
astreamselect           hue                     stereotools
asubboost               huesaturation           stereowiden
asubcut                 hwdownload              streamselect
asupercut               hwmap                   subtitles
asuperpass              hwupload                super2xsai
asuperstop              hwupload_cuda           superequalizer
atadenoise              hysteresis              surround
atempo                  identity                swaprect
atilt                   idet                    swapuv
atrim                   il                      tblend
avectorscope            inflate                 telecine
avgblur                 interlace               testsrc
avsynctest              interleave              testsrc2
axcorrelate             join                    thistogram
azmq                    kerndeint               threshold
backgroundkey           kirsch                  thumbnail
bandpass                lagfun                  thumbnail_cuda
bandreject              latency                 tile
bass                    lenscorrection          tiltandshift
bbox                    libvmaf                 tiltshelf
bench                   life                    tinterlace
bilateral               limitdiff               tlut2
bilateral_cuda          limiter                 tmedian
biquad                  loop                    tmidequalizer
bitplanenoise           loudnorm                tmix
blackdetect             lowpass                 tonemap
blackframe              lowshelf                tonemap_vaapi
blend                   lumakey                 tpad
blockdetect             lut                     transpose
blurdetect              lut1d                   transpose_vaapi
bm3d                    lut2                    treble
boxblur                 lut3d                   tremolo
bwdif                   lutrgb                  trim
bwdif_cuda              lutyuv                  unpremultiply
cas                     mandelbrot              unsharp
ccrepack                maskedclamp             untile
cellauto                maskedmax               uspp
channelmap              maskedmerge             v360
channelsplit            maskedmin               vaguedenoiser
chorus                  maskedthreshold         varblur
chromahold              maskfun                 vectorscope
chromakey               mcdeint                 vflip
chromakey_cuda          mcompand                vfrdet
chromanr                median                  vibrance
chromashift             mergeplanes             vibrato
ciescope                mestimate               vidstabdetect
codecview               metadata                vidstabtransform
color                   midequalizer            vif
colorbalance            minterpolate            vignette
colorchannelmixer       mix                     virtualbass
colorchart              monochrome              vmafmotion
colorcontrast           morpho                  volume
colorcorrect            movie                   volumedetect
colorhold               mpdecimate              vpp_qsv
colorize                mptestsrc               vstack
colorkey                msad                    vstack_qsv
colorlevels             multiply                vstack_vaapi
colormap                negate                  w3fdif
colormatrix             nlmeans                 waveform
colorspace              nnedi                   weave
colorspace_cuda         noformat                xbr
colorspectrum           noise                   xcorrelate
colortemperature        normalize               xfade
compand                 null                    xmedian
compensationdelay       nullsink                xpsnr
concat                  nullsrc                 xstack
convolution             oscilloscope            xstack_qsv
convolve                overlay                 xstack_vaapi
copy                    overlay_cuda            yadif
corr                    overlay_qsv             yadif_cuda
cover_rect              overlay_vaapi           yaepblur
crop                    owdenoise               yuvtestsrc
cropdetect              pad                     zmq
crossfeed               pad_vaapi               zoneplate
crystalizer             pal100bars              zoompan
cue                     pal75bars               zscale

Enabled bsfs:
aac_adtstoasc           h264_mp4toannexb        pcm_rechunk
av1_frame_merge         h264_redundant_pps      pgs_frame_merge
av1_frame_split         hapqa_extract           prores_metadata
av1_metadata            hevc_metadata           remove_extradata
chomp                   hevc_mp4toannexb        setts
dca_core                imx_dump_header         showinfo
dovi_rpu                media100_to_mjpegb      text2movsub
dts2pts                 mjpeg2jpeg              trace_headers
dump_extradata          mjpega_dump_header      truehd_core
dv_error_marker         mov2textsub             vp9_metadata
eac3_core               mpeg2_metadata          vp9_raw_reorder
evc_frame_merge         mpeg4_unpack_bframes    vp9_superframe
extract_extradata       noise                   vp9_superframe_split
filter_units            null                    vvc_metadata
h264_metadata           opus_metadata           vvc_mp4toannexb

Enabled indevs:
dshow                   lavfi
gdigrab                 vfwcap

Enabled outdevs:
sdl2

git-essentials external libraries' versions: 

AMF v1.4.35-3-g8f5a645
aom v3.11.0-110-g37c5c4e6aa
AviSynthPlus v3.7.3-80-g452cea05
ffnvcodec n12.2.72.0-1-g9934f17
freetype VER-2-13-3
fribidi v1.0.16
gsm 1.0.22
harfbuzz 10.1.0-17-g0b7beefd
lame 3.100
libass 0.17.3-38-g159cefc
libgme 0.6.3
libopencore-amrnb 0.1.6
libopencore-amrwb 0.1.6
libssh 0.11.1
libtheora 1.1.1
libwebp webp-rfc9649-8-g4c85d860
oneVPL 2.13
openmpt libopenmpt-0.6.20-19-gc8153cd77
opus v1.5.2-22-g7db26934
rubberband v1.8.1
SDL prerelease-2.29.2-432-gba433e4a5
speex Speex-1.2.1-29-gaca6801
srt v1.5.4-5-g5f16494
VAAPI 2.23.0.
vidstab v1.1.1-13-g8dff7ad
vmaf v3.0.0-99-g8cde19dc
vo-amrwbenc 0.1.3
vorbis v1.3.7-10-g84c02369
vpx v1.15.0-28-g6f0c446c7
x264 v0.164.3198
x265 4.1-54-gfa2770934
xvid v1.3.7
zeromq 4.3.5
zimg release-3.0.5-173-g30f368c

