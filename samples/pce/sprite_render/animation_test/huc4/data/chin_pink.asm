;#######################################################
;
; Generated by SPReD-PCE Copyright 2017-2022 0x8BitDev
;
;#######################################################

;_chin_pink_chr0:	.incbin "data/_chin_pink_chr0.bin"	; 1920 bytes
;_chin_pink_chr1:	.incbin "data/_chin_pink_chr1.bin"	; 1664 bytes
;_chin_pink_chr2:	.incbin "data/_chin_pink_chr2.bin"	; 1536 bytes
;_chin_pink_chr3:	.incbin "data/_chin_pink_chr3.bin"	; 1536 bytes
;_chin_pink_chr4:	.incbin "data/_chin_pink_chr4.bin"	; 1536 bytes

;_chin_pink_SG_arr:	
;	.word 1920, _chin_pink_chr0, bank(_chin_pink_chr0)
;	.word 1664, _chin_pink_chr1, bank(_chin_pink_chr1)
;	.word 1536, _chin_pink_chr2, bank(_chin_pink_chr2)
;	.word 1536, _chin_pink_chr3, bank(_chin_pink_chr3)
;	.word 1536, _chin_pink_chr4, bank(_chin_pink_chr4)


_chin_pink_palette:
_chin_pink_palette_slot0:
	.word $1C0, $1B, $24, $B6, $FE, $12E, $DB, $E0, $98, $EA, $1B5, $133, $A0, $1B6, $92, $00
_chin_pink_palette_end:

_chin_pink_num_frames:
	.byte $0A
_chin_pink_frames_data:
_chin_pink_idle01_RIGHT_frame:
	.word _chin_pink_idle01_RIGHT
	.byte bank(_chin_pink_idle01_RIGHT)
_chin_pink_walk01_RIGHT_frame:
	.word _chin_pink_walk01_RIGHT
	.byte bank(_chin_pink_walk01_RIGHT)
_chin_pink_walk02_RIGHT_frame:
	.word _chin_pink_walk02_RIGHT
	.byte bank(_chin_pink_walk02_RIGHT)
_chin_pink_walk03_RIGHT_frame:
	.word _chin_pink_walk03_RIGHT
	.byte bank(_chin_pink_walk03_RIGHT)
_chin_pink_walk04_RIGHT_frame:
	.word _chin_pink_walk04_RIGHT
	.byte bank(_chin_pink_walk04_RIGHT)
_chin_pink_idle01_LEFT_frame:
	.word _chin_pink_idle01_LEFT
	.byte bank(_chin_pink_idle01_LEFT)
_chin_pink_walk01_LEFT_frame:
	.word _chin_pink_walk01_LEFT
	.byte bank(_chin_pink_walk01_LEFT)
_chin_pink_walk02_LEFT_frame:
	.word _chin_pink_walk02_LEFT
	.byte bank(_chin_pink_walk02_LEFT)
_chin_pink_walk03_LEFT_frame:
	.word _chin_pink_walk03_LEFT
	.byte bank(_chin_pink_walk03_LEFT)
_chin_pink_walk04_LEFT_frame:
	.word _chin_pink_walk04_LEFT
	.byte bank(_chin_pink_walk04_LEFT)


	; #1: Y pos, #2: X pos, #3: CHR index, #4: CHR desc

_chin_pink_idle01_RIGHT:
	.word _chin_pink_idle01_RIGHT_end - _chin_pink_idle01_RIGHT - 3	; data size
	.byte 0		; GFX bank index (chr0)

	.word $FFB0, $FFE9, $100, $3181
	.word $FFF0, $FFE9, $110, $181
	.word $FFB0, $09, $114, $81
	.word $FFC0, $09, $116, $81
	.word $FFD0, $09, $118, $81
	.word $FFE0, $09, $11A, $81
	.word $FFF0, $09, $11C, $81
_chin_pink_idle01_RIGHT_end:

_chin_pink_walk01_RIGHT:
	.word _chin_pink_walk01_RIGHT_end - _chin_pink_walk01_RIGHT - 3	; data size
	.byte 1		; GFX bank index (chr1)

	.word $FFB0, $FFEC, $100, $3181
	.word $FFF0, $FFEC, $110, $181
	.word $FFC0, $0C, $114, $81
	.word $FFD0, $0C, $116, $81
	.word $FFF0, $0C, $118, $81
_chin_pink_walk01_RIGHT_end:

_chin_pink_walk02_RIGHT:
	.word _chin_pink_walk02_RIGHT_end - _chin_pink_walk02_RIGHT - 3	; data size
	.byte 2		; GFX bank index (chr2)

	.word $FFB0, $FFEC, $100, $3181
	.word $FFF0, $FFEC, $110, $181
	.word $FFC0, $0C, $114, $81
	.word $FFD0, $0C, $116, $81
_chin_pink_walk02_RIGHT_end:

_chin_pink_walk03_RIGHT:
	.word _chin_pink_walk03_RIGHT_end - _chin_pink_walk03_RIGHT - 3	; data size
	.byte 3		; GFX bank index (chr3)

	.word $FFB0, $FFEC, $100, $3181
	.word $FFF0, $FFEC, $110, $181
	.word $FFC0, $0C, $114, $81
	.word $FFD0, $0C, $116, $81
_chin_pink_walk03_RIGHT_end:

_chin_pink_walk04_RIGHT:
	.word _chin_pink_walk04_RIGHT_end - _chin_pink_walk04_RIGHT - 3	; data size
	.byte 4		; GFX bank index (chr4)

	.word $FFB0, $FFEC, $100, $3181
	.word $FFF0, $FFEC, $110, $181
	.word $FFC0, $0C, $114, $81
	.word $FFD0, $0C, $116, $81
_chin_pink_walk04_RIGHT_end:

_chin_pink_idle01_LEFT:
	.word _chin_pink_idle01_LEFT_end - _chin_pink_idle01_LEFT - 3	; data size
	.byte 0		; GFX bank index (chr0)

	.word $FFB0, $FFF7, $100, $3981
	.word $FFF0, $FFF7, $110, $981
	.word $FFB0, $FFE7, $114, $881
	.word $FFC0, $FFE7, $116, $881
	.word $FFD0, $FFE7, $118, $881
	.word $FFE0, $FFE7, $11A, $881
	.word $FFF0, $FFE7, $11C, $881
_chin_pink_idle01_LEFT_end:

_chin_pink_walk01_LEFT:
	.word _chin_pink_walk01_LEFT_end - _chin_pink_walk01_LEFT - 3	; data size
	.byte 1		; GFX bank index (chr1)

	.word $FFB0, $FFF4, $100, $3981
	.word $FFF0, $FFF4, $110, $981
	.word $FFC0, $FFE4, $114, $881
	.word $FFD0, $FFE4, $116, $881
	.word $FFF0, $FFE4, $118, $881
_chin_pink_walk01_LEFT_end:

_chin_pink_walk02_LEFT:
	.word _chin_pink_walk02_LEFT_end - _chin_pink_walk02_LEFT - 3	; data size
	.byte 2		; GFX bank index (chr2)

	.word $FFB0, $FFF4, $100, $3981
	.word $FFF0, $FFF4, $110, $981
	.word $FFC0, $FFE4, $114, $881
	.word $FFD0, $FFE4, $116, $881
_chin_pink_walk02_LEFT_end:

_chin_pink_walk03_LEFT:
	.word _chin_pink_walk03_LEFT_end - _chin_pink_walk03_LEFT - 3	; data size
	.byte 3		; GFX bank index (chr3)

	.word $FFB0, $FFF4, $100, $3981
	.word $FFF0, $FFF4, $110, $981
	.word $FFC0, $FFE4, $114, $881
	.word $FFD0, $FFE4, $116, $881
_chin_pink_walk03_LEFT_end:

_chin_pink_walk04_LEFT:
	.word _chin_pink_walk04_LEFT_end - _chin_pink_walk04_LEFT - 3	; data size
	.byte 4		; GFX bank index (chr4)

	.word $FFB0, $FFF4, $100, $3981
	.word $FFF0, $FFF4, $110, $981
	.word $FFC0, $FFE4, $114, $881
	.word $FFD0, $FFE4, $116, $881
_chin_pink_walk04_LEFT_end:

