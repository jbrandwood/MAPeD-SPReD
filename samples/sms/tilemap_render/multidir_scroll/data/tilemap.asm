;#######################################################
;
; Generated by MAPeD-SMS Copyright 2017-2022 0x8BitDev
;
;#######################################################

; export options:
;	- tiles 4x4 (RLE)/(columns)
;	- properties per CHR (screen attributes)
;	- mode: multidirectional scrolling
;	- layout: matrix (no marks)
;	- no entities


.define MAP_DATA_MAGIC $3202E

; data flags:
.define MAP_FLAG_TILES2X2                 $01
.define MAP_FLAG_TILES4X4                 $02
.define MAP_FLAG_RLE                      $04
.define MAP_FLAG_DIR_COLUMNS              $08
.define MAP_FLAG_DIR_ROWS                 $10
.define MAP_FLAG_MODE_MULTIDIR_SCROLL     $20
.define MAP_FLAG_MODE_BIDIR_SCROLL        $40
.define MAP_FLAG_MODE_STATIC_SCREENS      $80
.define MAP_FLAG_ENTITIES                 $100
.define MAP_FLAG_ENTITY_SCREEN_COORDS     $200
.define MAP_FLAG_ENTITY_MAP_COORS         $400
.define MAP_FLAG_LAYOUT_ADJACENT_SCREENS  $800
.define MAP_FLAG_LAYOUT_ADJACENT_SCR_INDS $1000
.define MAP_FLAG_LAYOUT_MATRIX            $2000
.define MAP_FLAG_MARKS                    $4000
.define MAP_FLAG_PROP_ID_PER_BLOCK        $8000
.define MAP_FLAG_PROP_ID_PER_CHR          $10000
.define MAP_FLAG_PROP_IN_SCR_ATTRS        $20000

.define	MAP_CHR_BPP	4
.define	MAP_CHRS_OFFSET	64	; first CHR index in CHR bank

.define SCR_BLOCKS2x2_WIDTH	16	; number of screen blocks (2x2) in width
.define SCR_BLOCKS2x2_HEIGHT	12	; number of screen blocks (2x2) in height

.define ScrTilesWidth	8	; number of screen tiles (4x4) in width
.define ScrTilesHeight	6	; number of screen tiles (4x4) in height

.define ScrPixelsWidth	256	; screen width in pixels
.define ScrPixelsHeight	192	; screen height in pixels

; *** GLOBAL DATA ***

.define Bank0_CHR_data_size	6208

Bank0_CHR:	.incbin "tilemap_Bank0_CHR.bin"		; (6208)
Bank0_Tiles:	.incbin "tilemap_Bank0_Tiles.bin"	; (392) (4x4) 4 block indices per tile ( left to right, up to down )
Bank0_Attrs:	.incbin "tilemap_Bank0_Attrs.bin"	; (1032) attributes array per block ( 2 bytes per attribute; 8 bytes per block )
Bank0_Palette:
		.byte $00, $23, $10, $14, $29, $18, $04, $09, $1E, $0F, $0B, $06, $01, $3F, $12, $3F
		.byte $00, $03, $07, $0B, $11, $14, $18, $1C, $22, $25, $29, $2D, $33, $36, $3A, $3E


; *** Lev0 ***

Lev0_Map:	.incbin "tilemap_Lev0_Map.bin"		; compressed (3343 / 4224) game level tiles (4x4) array
Lev0_MapTbl:	.incbin "tilemap_Lev0_MapTbl.bin"	; (128) lookup table for fast calculation of tile addresses; columns by X coordinate ( 16 bit offset per column of tiles )

.define Lev0_StartScr	8	; start screen
.define Lev0_WTilesCnt	64	; number of level tiles in width
.define Lev0_HTilesCnt	66	; number of level tiles in height
.define Lev0_WPixelsCnt	2048	; map width in pixels
.define Lev0_HPixelsCnt	2112	; map height in pixels
.define Lev0_TilesCnt	98	; map tiles count
.define Lev0_WScrCnt	8	; number of screens in width
.define Lev0_HScrCnt	11	; number of screens in height


MapsCHRBanks:
	.byte 0

