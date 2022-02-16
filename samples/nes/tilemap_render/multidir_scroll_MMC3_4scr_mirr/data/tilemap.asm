;#######################################################
;
; Generated by MAPeD-NES Copyright 2017-2022 0x8BitDev
;
;#######################################################

; export options:
;	- tiles 2x2/(columns)
;	- attributes per block
;	- properties per CHR
;	- mode: multidirectional scrolling
;	- layout: matrix (no marks)
;	- entities (map coordinates)


MAP_DATA_MAGIC = $4A529

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

Sonic:
	.byte $0F	; uid
	.byte $18	; width
	.byte $20	; height
	.byte $0C	; pivot x
	.byte $20	; pivot y
	.byte $00	; properties count

Burrobot:
	.byte $0A
	.byte $10
	.byte $20
	.byte $08
	.byte $20
	.byte $00

Jaws:
	.byte $0B
	.byte $18
	.byte $10
	.byte $00
	.byte $00
	.byte $00

Orbinaut:
	.byte $0C
	.byte $20
	.byte $20
	.byte $10
	.byte $10
	.byte $00

Spear:
	.byte $0D
	.byte $08
	.byte $30
	.byte $04
	.byte $30
	.byte $00

Thorny_Star:
	.byte $0E
	.byte $10
	.byte $10
	.byte $08
	.byte $08
	.byte $01
	.byte $48

Live_1Up:
	.byte $06
	.byte $20
	.byte $18
	.byte $10
	.byte $18
	.byte $00

Invincible:
	.byte $07
	.byte $20
	.byte $18
	.byte $10
	.byte $18
	.byte $00

Rings:
	.byte $08
	.byte $20
	.byte $18
	.byte $10
	.byte $18
	.byte $00

Shield:
	.byte $09
	.byte $20
	.byte $18
	.byte $10
	.byte $18
	.byte $00

Ring:
	.byte $00
	.byte $10
	.byte $10
	.byte $00
	.byte $00
	.byte $00

Chaos_Emeralds:
	.byte $01
	.byte $10
	.byte $10
	.byte $08
	.byte $10
	.byte $00

Checkpoint:
	.byte $02
	.byte $20
	.byte $18
	.byte $10
	.byte $18
	.byte $00

End_of_Level:
	.byte $03
	.byte $20
	.byte $28
	.byte $10
	.byte $28
	.byte $00

Oxygene:
	.byte $04
	.byte $18
	.byte $18
	.byte $00
	.byte $00
	.byte $00

Platform:
	.byte $05
	.byte $20
	.byte $10
	.byte $00
	.byte $00
	.byte $00


; *** GLOBAL DATA ***

; Bank0_CHR:	.incbin "tilemap_Bank0_CHR.bin"		; (3264)
Bank0_Blocks:	.incbin "tilemap_Bank0_Blocks.bin"	; (516) blocks data ( 4 CHR indices per block; left to right, up to down )
Bank0_Props:	.incbin "tilemap_Bank0_Props.bin"	; (516) block properties array ( 4 bytes per block )

Bank0_Palette:
		.byte $0D, $39, $28, $18, $0D, $0C, $31, $1C, $0D, $1B, $2A, $00, $0D, $05, $14, $20


; *** Lev0 ***

Lev0_Map:	.incbin "tilemap_Lev0_Map.bin"		; (15360) game level blocks (2x2) array
Lev0_MapTbl:	.incbin "tilemap_Lev0_MapTbl.bin"	; (256) lookup table for fast calculation of tile addresses columns by X coordinate ( 16 bit offset per column of tiles )
Lev0_AttrsMap:	.incbin "tilemap_Lev0_AttrsMap.bin"	; (4096) map of attributes ( 1 byte per attribute )

Lev0_StartScr	=	10	; start screen
Lev0_WTilesCnt	=	128	; number of level tiles in width
Lev0_HTilesCnt	=	120	; number of level tiles in height
Lev0_TilesCnt	=	129	; map tiles count
.define Lev0_WScrCnt	8	; number of screens in width
.define Lev0_HScrCnt	8	; number of screens in height

Lev0_Layout:	
	.word Lev0Scr0, Lev0Scr1, Lev0Scr2, Lev0Scr3, Lev0Scr4, Lev0Scr5, Lev0Scr6, Lev0Scr7
	.word Lev0Scr8, Lev0Scr9, Lev0Scr10, Lev0Scr11, Lev0Scr12, Lev0Scr13, Lev0Scr14, Lev0Scr15
	.word Lev0Scr16, Lev0Scr17, Lev0Scr18, Lev0Scr19, Lev0Scr20, Lev0Scr21, Lev0Scr22, Lev0Scr23
	.word Lev0Scr24, Lev0Scr25, Lev0Scr26, Lev0Scr27, Lev0Scr28, Lev0Scr29, Lev0Scr30, Lev0Scr31
	.word Lev0Scr32, Lev0Scr33, Lev0Scr34, Lev0Scr35, Lev0Scr36, Lev0Scr37, Lev0Scr38, Lev0Scr39
	.word Lev0Scr40, Lev0Scr41, Lev0Scr42, Lev0Scr43, Lev0Scr44, Lev0Scr45, Lev0Scr46, Lev0Scr47
	.word Lev0Scr48, Lev0Scr49, Lev0Scr50, Lev0Scr51, Lev0Scr52, Lev0Scr53, Lev0Scr54, Lev0Scr55
	.word Lev0Scr56, Lev0Scr57, Lev0Scr58, Lev0Scr59, Lev0Scr60, Lev0Scr61, Lev0Scr62, Lev0Scr63

Lev0Scr0:
Lev0Scr0EntsArr:
	.byte 0

Lev0Scr1:
Lev0Scr1EntsArr:
	.byte 0

Lev0Scr2:
Lev0Scr2EntsArr:
	.byte 0

Lev0Scr3:
Lev0Scr3EntsArr:
	.byte 0

Lev0Scr4:
Lev0Scr4EntsArr:
	.byte 0

Lev0Scr5:
Lev0Scr5EntsArr:
	.byte 0

Lev0Scr6:
Lev0Scr6EntsArr:
	.byte 0

Lev0Scr7:
Lev0Scr7EntsArr:
	.byte 0

Lev0Scr8:
Lev0Scr8EntsArr:
	.byte 0

Lev0Scr9:
Lev0Scr9EntsArr:
	.byte 0

Lev0Scr10:
Lev0Scr10EntsArr:
	.byte 1
	.word Instance1
Instance1:
	.byte $00
	.word Sonic
	.word $00
	.word $240
	.word $170
	.byte $00

Lev0Scr11:
Lev0Scr11EntsArr:
	.byte 0

Lev0Scr12:
Lev0Scr12EntsArr:
	.byte 0

Lev0Scr13:
Lev0Scr13EntsArr:
	.byte 0

Lev0Scr14:
Lev0Scr14EntsArr:
	.byte 0

Lev0Scr15:
Lev0Scr15EntsArr:
	.byte 1
	.word Instance117
Instance117:
	.byte $01
	.word End_of_Level
	.word $00
	.word $710
	.word $1B0
	.byte $00

Lev0Scr16:
Lev0Scr16EntsArr:
	.byte 0

Lev0Scr17:
Lev0Scr17EntsArr:
	.byte 0

Lev0Scr18:
Lev0Scr18EntsArr:
	.byte 1
	.word Instance2
Instance2:
	.byte $02
	.word Shield
	.word $00
	.word $230
	.word $2B0
	.byte $00

Lev0Scr19:
Lev0Scr19EntsArr:
	.byte 0

Lev0Scr20:
Lev0Scr20EntsArr:
	.byte 0

Lev0Scr21:
Lev0Scr21EntsArr:
	.byte 3
	.word Instance110
	.word Instance111
	.word Instance112
Instance110:
	.byte $03
	.word Chaos_Emeralds
	.word $00
	.word $570
	.word $2D0
	.byte $00
Instance111:
	.byte $04
	.word Ring
	.word $00
	.word $5C0
	.word $288
	.byte $00
Instance112:
	.byte $05
	.word Ring
	.word $00
	.word $5D0
	.word $288
	.byte $00

Lev0Scr22:
Lev0Scr22EntsArr:
	.byte 4
	.word Instance113
	.word Instance114
	.word Instance115
	.word Instance116
Instance113:
	.byte $06
	.word Ring
	.word $00
	.word $630
	.word $268
	.byte $00
Instance114:
	.byte $07
	.word Ring
	.word $00
	.word $620
	.word $268
	.byte $00
Instance115:
	.byte $08
	.word Ring
	.word $00
	.word $620
	.word $288
	.byte $00
Instance116:
	.byte $09
	.word Ring
	.word $00
	.word $630
	.word $288
	.byte $00

Lev0Scr23:
Lev0Scr23EntsArr:
	.byte 0

Lev0Scr24:
Lev0Scr24EntsArr:
	.byte 0

Lev0Scr25:
Lev0Scr25EntsArr:
	.byte 8
	.word Instance3
	.word Instance4
	.word Instance5
	.word Instance6
	.word Instance13
	.word Instance14
	.word Instance15
	.word Instance16
Instance3:
	.byte $0A
	.word Ring
	.word $00
	.word $1C0
	.word $2E8
	.byte $00
Instance4:
	.byte $0B
	.word Ring
	.word $00
	.word $1D0
	.word $2E8
	.byte $00
Instance5:
	.byte $0C
	.word Ring
	.word $00
	.word $1E0
	.word $2E8
	.byte $00
Instance6:
	.byte $0D
	.word Ring
	.word $00
	.word $1F0
	.word $2E8
	.byte $00
Instance13:
	.byte $0E
	.word Ring
	.word $00
	.word $1C0
	.word $308
	.byte $00
Instance14:
	.byte $0F
	.word Ring
	.word $00
	.word $1D0
	.word $308
	.byte $00
Instance15:
	.byte $10
	.word Ring
	.word $00
	.word $1E0
	.word $308
	.byte $00
Instance16:
	.byte $11
	.word Ring
	.word $00
	.word $1F0
	.word $308
	.byte $00

Lev0Scr26:
Lev0Scr26EntsArr:
	.byte 13
	.word Instance7
	.word Instance8
	.word Instance9
	.word Instance10
	.word Instance11
	.word Instance12
	.word Instance17
	.word Instance18
	.word Instance19
	.word Instance20
	.word Instance21
	.word Instance22
	.word Instance23
Instance7:
	.byte $12
	.word Ring
	.word $00
	.word $200
	.word $2E8
	.byte $00
Instance8:
	.byte $13
	.word Ring
	.word $00
	.word $210
	.word $2E8
	.byte $00
Instance9:
	.byte $14
	.word Ring
	.word $00
	.word $220
	.word $2E8
	.byte $00
Instance10:
	.byte $15
	.word Ring
	.word $00
	.word $230
	.word $2E8
	.byte $00
Instance11:
	.byte $16
	.word Ring
	.word $00
	.word $240
	.word $2E8
	.byte $00
Instance12:
	.byte $17
	.word Ring
	.word $00
	.word $250
	.word $2E8
	.byte $00
Instance17:
	.byte $18
	.word Ring
	.word $00
	.word $200
	.word $308
	.byte $00
Instance18:
	.byte $19
	.word Ring
	.word $00
	.word $210
	.word $308
	.byte $00
Instance19:
	.byte $1A
	.word Ring
	.word $00
	.word $220
	.word $308
	.byte $00
Instance20:
	.byte $1B
	.word Ring
	.word $00
	.word $230
	.word $308
	.byte $00
Instance21:
	.byte $1C
	.word Ring
	.word $00
	.word $240
	.word $308
	.byte $00
Instance22:
	.byte $1D
	.word Ring
	.word $00
	.word $250
	.word $308
	.byte $00
Instance23:
	.byte $1E
	.word Rings
	.word $00
	.word $270
	.word $3B0
	.byte $00

Lev0Scr27:
Lev0Scr27EntsArr:
	.byte 0

Lev0Scr28:
Lev0Scr28EntsArr:
	.byte 0

Lev0Scr29:
Lev0Scr29EntsArr:
	.byte 1
	.word Instance109
Instance109:
	.byte $1F
	.word Invincible
	.word $00
	.word $5C0
	.word $3B0
	.byte $00

Lev0Scr30:
Lev0Scr30EntsArr:
	.byte 0

Lev0Scr31:
Lev0Scr31EntsArr:
	.byte 0

Lev0Scr32:
Lev0Scr32EntsArr:
	.byte 0

Lev0Scr33:
Lev0Scr33EntsArr:
	.byte 0

Lev0Scr34:
Lev0Scr34EntsArr:
	.byte 1
	.word Instance24
Instance24:
	.byte $20
	.word Jaws
	.word $00
	.word $250
	.word $440
	.byte $02
	.byte $01, $C0

Lev0Scr35:
Lev0Scr35EntsArr:
	.byte 0

Lev0Scr36:
Lev0Scr36EntsArr:
	.byte 21
	.word Instance41
	.word Instance42
	.word Instance43
	.word Instance44
	.word Instance45
	.word Instance46
	.word Instance47
	.word Instance49
	.word Instance50
	.word Instance51
	.word Instance52
	.word Instance53
	.word Instance54
	.word Instance55
	.word Instance56
	.word Instance57
	.word Instance58
	.word Instance59
	.word Instance60
	.word Instance61
	.word Instance62
Instance41:
	.byte $21
	.word Orbinaut
	.word $00
	.word $440
	.word $478
	.byte $00
Instance42:
	.byte $22
	.word Ring
	.word $00
	.word $490
	.word $3E8
	.byte $00
Instance43:
	.byte $23
	.word Ring
	.word $00
	.word $4A0
	.word $3E8
	.byte $00
Instance44:
	.byte $24
	.word Ring
	.word $00
	.word $4B0
	.word $3E8
	.byte $00
Instance45:
	.byte $25
	.word Ring
	.word $00
	.word $4C0
	.word $3E8
	.byte $00
Instance46:
	.byte $26
	.word Ring
	.word $00
	.word $4D0
	.word $3E8
	.byte $00
Instance47:
	.byte $27
	.word Ring
	.word $00
	.word $4E0
	.word $3E8
	.byte $00
Instance49:
	.byte $28
	.word Ring
	.word $00
	.word $480
	.word $408
	.byte $00
Instance50:
	.byte $29
	.word Ring
	.word $00
	.word $490
	.word $408
	.byte $00
Instance51:
	.byte $2A
	.word Ring
	.word $00
	.word $4A0
	.word $408
	.byte $00
Instance52:
	.byte $2B
	.word Ring
	.word $00
	.word $4B0
	.word $408
	.byte $00
Instance53:
	.byte $2C
	.word Ring
	.word $00
	.word $4C0
	.word $408
	.byte $00
Instance54:
	.byte $2D
	.word Ring
	.word $00
	.word $4D0
	.word $408
	.byte $00
Instance55:
	.byte $2E
	.word Ring
	.word $00
	.word $4E0
	.word $408
	.byte $00
Instance56:
	.byte $2F
	.word Ring
	.word $00
	.word $4F0
	.word $408
	.byte $00
Instance57:
	.byte $30
	.word Ring
	.word $00
	.word $490
	.word $428
	.byte $00
Instance58:
	.byte $31
	.word Ring
	.word $00
	.word $4A0
	.word $428
	.byte $00
Instance59:
	.byte $32
	.word Ring
	.word $00
	.word $4B0
	.word $428
	.byte $00
Instance60:
	.byte $33
	.word Ring
	.word $00
	.word $4C0
	.word $428
	.byte $00
Instance61:
	.byte $34
	.word Ring
	.word $00
	.word $4D0
	.word $428
	.byte $00
Instance62:
	.byte $35
	.word Ring
	.word $00
	.word $4E0
	.word $428
	.byte $00

Lev0Scr37:
Lev0Scr37EntsArr:
	.byte 1
	.word Instance108
Instance108:
	.byte $36
	.word Thorny_Star
	.word $00
	.word $598
	.word $450
	.byte $00

Lev0Scr38:
Lev0Scr38EntsArr:
	.byte 0

Lev0Scr39:
Lev0Scr39EntsArr:
	.byte 0

Lev0Scr40:
Lev0Scr40EntsArr:
	.byte 0

Lev0Scr41:
Lev0Scr41EntsArr:
	.byte 1
	.word Instance25
Instance25:
	.byte $37
	.word Burrobot
	.word $00
	.word $1F8
	.word $510
	.byte $00

Lev0Scr42:
Lev0Scr42EntsArr:
	.byte 2
	.word Instance26
	.word Instance27
Instance26:
	.byte $38
	.word Jaws
	.word $00
	.word $2E0
	.word $540
	.byte $00
Instance27:
	.byte $39
	.word Oxygene
	.word $00
	.word $2A0
	.word $558
	.byte $00

Lev0Scr43:
Lev0Scr43EntsArr:
	.byte 0

Lev0Scr44:
Lev0Scr44EntsArr:
	.byte 0

Lev0Scr45:
Lev0Scr45EntsArr:
	.byte 5
	.word Instance83
	.word Instance84
	.word Instance85
	.word Instance86
	.word Instance107
Instance83:
	.byte $3A
	.word Ring
	.word $00
	.word $5F0
	.word $588
	.byte $00
Instance84:
	.byte $3B
	.word Ring
	.word $00
	.word $5E0
	.word $588
	.byte $00
Instance85:
	.byte $3C
	.word Ring
	.word $00
	.word $5D0
	.word $588
	.byte $00
Instance86:
	.byte $3D
	.word Ring
	.word $00
	.word $5C0
	.word $588
	.byte $00
Instance107:
	.byte $3E
	.word Jaws
	.word $00
	.word $568
	.word $588
	.byte $02
	.byte $01, $C0

Lev0Scr46:
Lev0Scr46EntsArr:
	.byte 1
	.word Instance106
Instance106:
	.byte $3F
	.word Oxygene
	.word $00
	.word $680
	.word $4D8
	.byte $00

Lev0Scr47:
Lev0Scr47EntsArr:
	.byte 0

Lev0Scr48:
Lev0Scr48EntsArr:
	.byte 0

Lev0Scr49:
Lev0Scr49EntsArr:
	.byte 10
	.word Instance28
	.word Instance29
	.word Instance30
	.word Instance31
	.word Instance32
	.word Instance33
	.word Instance34
	.word Instance35
	.word Instance36
	.word Instance37
Instance28:
	.byte $40
	.word Ring
	.word $00
	.word $1C0
	.word $5E8
	.byte $00
Instance29:
	.byte $41
	.word Ring
	.word $00
	.word $1D0
	.word $5E8
	.byte $00
Instance30:
	.byte $42
	.word Ring
	.word $00
	.word $1C0
	.word $608
	.byte $00
Instance31:
	.byte $43
	.word Ring
	.word $00
	.word $1D0
	.word $608
	.byte $00
Instance32:
	.byte $44
	.word Ring
	.word $00
	.word $1C0
	.word $628
	.byte $00
Instance33:
	.byte $45
	.word Ring
	.word $00
	.word $1D0
	.word $628
	.byte $00
Instance34:
	.byte $46
	.word Ring
	.word $00
	.word $1C0
	.word $648
	.byte $00
Instance35:
	.byte $47
	.word Ring
	.word $00
	.word $1D0
	.word $648
	.byte $00
Instance36:
	.byte $48
	.word Ring
	.word $00
	.word $1C0
	.word $668
	.byte $00
Instance37:
	.byte $49
	.word Ring
	.word $00
	.word $1D0
	.word $668
	.byte $00

Lev0Scr50:
Lev0Scr50EntsArr:
	.byte 0

Lev0Scr51:
Lev0Scr51EntsArr:
	.byte 0

Lev0Scr52:
Lev0Scr52EntsArr:
	.byte 15
	.word Instance63
	.word Instance64
	.word Instance65
	.word Instance66
	.word Instance67
	.word Instance68
	.word Instance69
	.word Instance70
	.word Instance71
	.word Instance72
	.word Instance73
	.word Instance74
	.word Instance75
	.word Instance76
	.word Instance77
Instance63:
	.byte $4A
	.word Oxygene
	.word $00
	.word $420
	.word $678
	.byte $00
Instance64:
	.byte $4B
	.word Spear
	.word $00
	.word $470
	.word $690
	.byte $00
Instance65:
	.byte $4C
	.word Spear
	.word $00
	.word $4F0
	.word $690
	.byte $00
Instance66:
	.byte $4D
	.word Ring
	.word $00
	.word $440
	.word $648
	.byte $00
Instance67:
	.byte $4E
	.word Ring
	.word $00
	.word $450
	.word $648
	.byte $00
Instance68:
	.byte $4F
	.word Ring
	.word $00
	.word $460
	.word $648
	.byte $00
Instance69:
	.byte $50
	.word Ring
	.word $00
	.word $470
	.word $648
	.byte $00
Instance70:
	.byte $51
	.word Ring
	.word $00
	.word $480
	.word $648
	.byte $00
Instance71:
	.byte $52
	.word Ring
	.word $00
	.word $490
	.word $648
	.byte $00
Instance72:
	.byte $53
	.word Ring
	.word $00
	.word $4A0
	.word $648
	.byte $00
Instance73:
	.byte $54
	.word Ring
	.word $00
	.word $4B0
	.word $648
	.byte $00
Instance74:
	.byte $55
	.word Ring
	.word $00
	.word $4C0
	.word $648
	.byte $00
Instance75:
	.byte $56
	.word Ring
	.word $00
	.word $4D0
	.word $648
	.byte $00
Instance76:
	.byte $57
	.word Ring
	.word $00
	.word $4E0
	.word $648
	.byte $00
Instance77:
	.byte $58
	.word Ring
	.word $00
	.word $4F0
	.word $648
	.byte $00

Lev0Scr53:
Lev0Scr53EntsArr:
	.byte 8
	.word Instance79
	.word Instance80
	.word Instance81
	.word Instance82
	.word Instance87
	.word Instance88
	.word Instance89
	.word Instance90
Instance79:
	.byte $59
	.word Ring
	.word $00
	.word $550
	.word $628
	.byte $00
Instance80:
	.byte $5A
	.word Ring
	.word $00
	.word $540
	.word $628
	.byte $00
Instance81:
	.byte $5B
	.word Ring
	.word $00
	.word $570
	.word $5E8
	.byte $00
Instance82:
	.byte $5C
	.word Ring
	.word $00
	.word $560
	.word $5E8
	.byte $00
Instance87:
	.byte $5D
	.word Ring
	.word $00
	.word $5C0
	.word $5A8
	.byte $00
Instance88:
	.byte $5E
	.word Ring
	.word $00
	.word $5D0
	.word $5A8
	.byte $00
Instance89:
	.byte $5F
	.word Ring
	.word $00
	.word $5E0
	.word $5A8
	.byte $00
Instance90:
	.byte $60
	.word Ring
	.word $00
	.word $5F0
	.word $5A8
	.byte $00

Lev0Scr54:
Lev0Scr54EntsArr:
	.byte 11
	.word Instance91
	.word Instance92
	.word Instance93
	.word Instance94
	.word Instance95
	.word Instance96
	.word Instance97
	.word Instance98
	.word Instance99
	.word Instance100
	.word Instance105
Instance91:
	.byte $61
	.word Ring
	.word $00
	.word $640
	.word $5E8
	.byte $00
Instance92:
	.byte $62
	.word Ring
	.word $00
	.word $650
	.word $5E8
	.byte $00
Instance93:
	.byte $63
	.word Ring
	.word $00
	.word $660
	.word $628
	.byte $00
Instance94:
	.byte $64
	.word Ring
	.word $00
	.word $670
	.word $628
	.byte $00
Instance95:
	.byte $65
	.word Ring
	.word $00
	.word $6A0
	.word $668
	.byte $00
Instance96:
	.byte $66
	.word Ring
	.word $00
	.word $6B0
	.word $668
	.byte $00
Instance97:
	.byte $67
	.word Ring
	.word $00
	.word $6C0
	.word $668
	.byte $00
Instance98:
	.byte $68
	.word Ring
	.word $00
	.word $6D0
	.word $668
	.byte $00
Instance99:
	.byte $69
	.word Ring
	.word $00
	.word $6E0
	.word $668
	.byte $00
Instance100:
	.byte $6A
	.word Ring
	.word $00
	.word $6F0
	.word $668
	.byte $00
Instance105:
	.byte $6B
	.word Oxygene
	.word $00
	.word $600
	.word $5B8
	.byte $00

Lev0Scr55:
Lev0Scr55EntsArr:
	.byte 4
	.word Instance101
	.word Instance102
	.word Instance103
	.word Instance104
Instance101:
	.byte $6C
	.word Ring
	.word $00
	.word $700
	.word $668
	.byte $00
Instance102:
	.byte $6D
	.word Ring
	.word $00
	.word $710
	.word $668
	.byte $00
Instance103:
	.byte $6E
	.word Ring
	.word $00
	.word $720
	.word $668
	.byte $00
Instance104:
	.byte $6F
	.word Ring
	.word $00
	.word $730
	.word $668
	.byte $00

Lev0Scr56:
Lev0Scr56EntsArr:
	.byte 1
	.word Instance40
Instance40:
	.byte $70
	.word Live_1Up
	.word $00
	.word $B0
	.word $750
	.byte $00

Lev0Scr57:
Lev0Scr57EntsArr:
	.byte 2
	.word Instance39
	.word Instance38
Instance39:
	.byte $71
	.word Platform
	.word $00
	.word $140
	.word $750
	.byte $02
	.byte $00, $80
Instance38:
	.byte $72
	.word Platform
	.word $00
	.word $1C0
	.word $750
	.byte $03
	.byte $00, $00, $02

Lev0Scr58:
Lev0Scr58EntsArr:
	.byte 0

Lev0Scr59:
Lev0Scr59EntsArr:
	.byte 0

Lev0Scr60:
Lev0Scr60EntsArr:
	.byte 0

Lev0Scr61:
Lev0Scr61EntsArr:
	.byte 1
	.word Instance78
Instance78:
	.byte $73
	.word Checkpoint
	.word $00
	.word $530
	.word $6A8
	.byte $00

Lev0Scr62:
Lev0Scr62EntsArr:
	.byte 0

Lev0Scr63:
Lev0Scr63EntsArr:
	.byte 1
	.word Instance118
Instance118:
	.byte $74
	.word Platform
	.word $00
	.word $740
	.word $690
	.byte $03
	.byte $00, $E0, $01

.define Lev0_EntInstCnt	117	; number of entities instances

MapsCHRBanks:
	.byte 0

