;#######################################################
;
; Generated by MAPeD-NES Copyright 2017-2021 0x8BitDev
;
;#######################################################

; export options:
;	- tiles 4x4 (RLE)/(columns)
;	- attributes per block
;	- properties per block
;	- mode: multidirectional scrolling
;	- layout: matrix (no marks)
;	- entities (screen coordinates)


MAP_DATA_MAGIC = $2A32E

; data flags:
MAP_FLAG_TILES2X2                 = $01
MAP_FLAG_TILES4X4                 = $02
MAP_FLAG_RLE                      = $04
MAP_FLAG_DIR_COLUMNS              = $08
MAP_FLAG_DIR_ROWS                 = $10
MAP_FLAG_MODE_MULTIDIR_SCROLL     = $20
MAP_FLAG_MODE_BIDIR_SCROLL        = $40
MAP_FLAG_MODE_STATIC_SCREENS      = $80
MAP_FLAG_ENTITIES                 = $100
MAP_FLAG_ENTITY_SCREEN_COORDS     = $200
MAP_FLAG_ENTITY_MAP_COORS         = $400
MAP_FLAG_LAYOUT_ADJACENT_SCREENS  = $800
MAP_FLAG_LAYOUT_ADJACENT_SCR_INDS = $1000
MAP_FLAG_LAYOUT_MATRIX            = $2000
MAP_FLAG_MARKS                    = $4000
MAP_FLAG_ATTRS_PER_BLOCK          = $8000
MAP_FLAG_ATTRS_PER_CHR            = $10000
MAP_FLAG_PROP_ID_PER_BLOCK        = $20000
MAP_FLAG_PROP_ID_PER_CHR          = $40000

SCR_BLOCKS2x2_WIDTH	= 16	; number of screen blocks (2x2) in width
SCR_BLOCKS2x2_HEIGHT	= 15	; number of screen blocks (2x2) in height

; *** BASE ENTITIES ***

bobik_zdoh:
	.byte $00	; uid
	.byte $18	; width
	.byte $20	; height
	.byte $0C	; pivot x
	.byte $20	; pivot y
	.byte $03	; properties count
	.byte $23, $01, $45	; properties data

walking_robot01:
	.byte $01
	.byte $18
	.byte $20
	.byte $0C
	.byte $20
	.byte $01
	.byte $80

crystal01:
	.byte $02
	.byte $10
	.byte $10
	.byte $08
	.byte $10
	.byte $00

energy01:
	.byte $03
	.byte $10
	.byte $10
	.byte $08
	.byte $10
	.byte $02
	.byte $00, $01

lift01:
	.byte $04
	.byte $40
	.byte $08
	.byte $20
	.byte $00
	.byte $00

switch01:
	.byte $05
	.byte $10
	.byte $10
	.byte $08
	.byte $10
	.byte $00


; *** Lev0 ***

; Lev0_CHR:	.incbin "tilemap_Lev0_CHR.bin"		; (1408)
Lev0_Tiles:	.incbin "tilemap_Lev0_Tiles.bin"	; (676) (4x4) 4 block indices per tile ( left to right, up to down )
Lev0_Attrs:	.incbin "tilemap_Lev0_Attrs.bin"	; (169) attributes array ( 1 byte per tile )
Lev0_Blocks:	.incbin "tilemap_Lev0_Blocks.bin"	; (180) blocks data ( 4 CHR indices per block; left to right, up to down )
Lev0_Props:	.incbin "tilemap_Lev0_Props.bin"	; (45) block properties array ( 1 byte per block )
Lev0_Map:	.incbin "tilemap_Lev0_Map.bin"		; compressed (753 / 768) game level tiles (4x4) array
Lev0_MapTbl:	.incbin "tilemap_Lev0_MapTbl.bin"	; (64) lookup table for fast calculation of tile addresses columns by X coordinate ( 16 bit offset per column of tiles )

Lev0_Palette:
		.byte $22, $08, $16, $27, $22, $0C, $1C, $31, $22, $0B, $1B, $2B, $22, $08, $06, $27

Lev0_StartScr	=	0	; start screen
Lev0_WTilesCnt	=	32	; number of level tiles in width
Lev0_HTilesCnt	=	24	; number of level tiles in height
Lev0_TilesCnt	=	169
Lev0_BlocksCnt	=	45

.define Lev0_WScrCnt	4	; number of screens in width
.define Lev0_HScrCnt	3	; number of screens in height

Lev0_Layout:	
	.word Lev0Scr0, Lev0Scr1, Lev0Scr2, Lev0Scr3
	.word Lev0Scr4, Lev0Scr5, Lev0Scr6, Lev0Scr7
	.word Lev0Scr8, Lev0Scr9, Lev0Scr10, Lev0Scr11

Lev0Scr0:
Lev0Scr0EntsArr:
	.byte 3
	.word Instance48
	.word Instance39
	.word Instance65
Instance48:
	.byte $00	; entity instance number (0..n)
	.word crystal01	; base entity
	.word $00	; target entity
	.word $40	; scr X
	.word $60	; scr Y
	.byte $00	; properties count
Instance39:
	.byte $01	; entity instance number (0..n)
	.word switch01	; base entity
	.word Instance34	; target entity
	.word $D0	; scr X
	.word $60	; scr Y
	.byte $00	; properties count
Instance65:
	.byte $02	; entity instance number (0..n)
	.word bobik_zdoh	; base entity
	.word $00	; target entity
	.word $88	; scr X
	.word $C0	; scr Y
	.byte $00	; properties count

Lev0Scr1:
Lev0Scr1EntsArr:
	.byte 4
	.word Instance49
	.word Instance59
	.word Instance44
	.word Instance34
Instance49:
	.byte $03
	.word crystal01
	.word $00
	.word $20
	.word $60
	.byte $00
Instance59:
	.byte $04
	.word walking_robot01
	.word $00
	.word $B0
	.word $C0
	.byte $00
Instance44:
	.byte $05
	.word energy01
	.word $00
	.word $D8
	.word $60
	.byte $00
Instance34:
	.byte $06
	.word lift01
	.word $00
	.word $20
	.word $C0
	.byte $02
	.byte $01, $80

Lev0Scr2:
Lev0Scr2EntsArr:
	.byte 1
	.word Instance50
Instance50:
	.byte $07
	.word crystal01
	.word $00
	.word $70
	.word $60
	.byte $00

Lev0Scr3:
Lev0Scr3EntsArr:
	.byte 2
	.word Instance51
	.word Instance38
Instance51:
	.byte $08
	.word crystal01
	.word $00
	.word $18
	.word $60
	.byte $00
Instance38:
	.byte $09
	.word lift01
	.word $00
	.word $C0
	.word $C0
	.byte $02
	.byte $02, $96

Lev0Scr4:
Lev0Scr4EntsArr:
	.byte 3
	.word Instance52
	.word Instance53
	.word Instance62
Instance52:
	.byte $0A
	.word crystal01
	.word $00
	.word $98
	.word $80
	.byte $00
Instance53:
	.byte $0B
	.word crystal01
	.word $00
	.word $40
	.word $E0
	.byte $00
Instance62:
	.byte $0C
	.word walking_robot01
	.word $00
	.word $88
	.word $E0
	.byte $00

Lev0Scr5:
Lev0Scr5EntsArr:
	.byte 1
	.word Instance61
Instance61:
	.byte $0D
	.word walking_robot01
	.word $00
	.word $B0
	.word $80
	.byte $00

Lev0Scr6:
Lev0Scr6EntsArr:
	.byte 5
	.word Instance40
	.word Instance45
	.word Instance54
	.word Instance60
	.word Instance36
Instance40:
	.byte $0E
	.word switch01
	.word Instance36
	.word $28
	.word $80
	.byte $00
Instance45:
	.byte $0F
	.word energy01
	.word $00
	.word $C0
	.word $20
	.byte $00
Instance54:
	.byte $10
	.word crystal01
	.word $00
	.word $C0
	.word $E0
	.byte $00
Instance60:
	.byte $11
	.word walking_robot01
	.word $00
	.word $88
	.word $20
	.byte $00
Instance36:
	.byte $12
	.word lift01
	.word $00
	.word $60
	.word $E0
	.byte $02
	.byte $02, $96

Lev0Scr7:
Lev0Scr7EntsArr:
	.byte 2
	.word Instance43
	.word Instance37
Instance43:
	.byte $13
	.word switch01
	.word Instance38
	.word $20
	.word $80
	.byte $00
Instance37:
	.byte $14
	.word lift01
	.word $00
	.word $50
	.word $E0
	.byte $02
	.byte $01, $96

Lev0Scr8:
Lev0Scr8EntsArr:
	.byte 3
	.word Instance46
	.word Instance55
	.word Instance63
Instance46:
	.byte $15
	.word energy01
	.word $00
	.word $E8
	.word $50
	.byte $00
Instance55:
	.byte $16
	.word crystal01
	.word $00
	.word $70
	.word $50
	.byte $00
Instance63:
	.byte $17
	.word walking_robot01
	.word $00
	.word $60
	.word $C0
	.byte $00

Lev0Scr9:
Lev0Scr9EntsArr:
	.byte 3
	.word Instance42
	.word Instance56
	.word Instance35
Instance42:
	.byte $18
	.word switch01
	.word Instance35
	.word $F8
	.word $C0
	.byte $00
Instance56:
	.byte $19
	.word crystal01
	.word $00
	.word $38
	.word $C0
	.byte $00
Instance35:
	.byte $1A
	.word lift01
	.word $00
	.word $C0
	.word $50
	.byte $03
	.byte $01, $12, $01

Lev0Scr10:
Lev0Scr10EntsArr:
	.byte 3
	.word Instance41
	.word Instance57
	.word Instance64
Instance41:
	.byte $1B
	.word switch01
	.word Instance37
	.word $E0
	.word $50
	.byte $00
Instance57:
	.byte $1C
	.word crystal01
	.word $00
	.word $30
	.word $50
	.byte $00
Instance64:
	.byte $1D
	.word walking_robot01
	.word $00
	.word $98
	.word $C0
	.byte $00

Lev0Scr11:
Lev0Scr11EntsArr:
	.byte 2
	.word Instance47
	.word Instance58
Instance47:
	.byte $1E
	.word energy01
	.word $00
	.word $C0
	.word $C0
	.byte $00
Instance58:
	.byte $1F
	.word crystal01
	.word $00
	.word $90
	.word $50
	.byte $00

.define Lev0_EntInstCnt	32	; number of entities instances
