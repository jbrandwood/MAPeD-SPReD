;#######################################################
;
; Generated by SPReD-PCE Copyright 2017-2022 0x8BitDev
;
;#######################################################

;_chin_blue_chr0:	.incbin "data/_chin_blue_chr0.bin"	; 1920 bytes
;_chin_blue_chr1:	.incbin "data/_chin_blue_chr1.bin"	; 1664 bytes
;_chin_blue_chr2:	.incbin "data/_chin_blue_chr2.bin"	; 1536 bytes
;_chin_blue_chr3:	.incbin "data/_chin_blue_chr3.bin"	; 1536 bytes
;_chin_blue_chr4:	.incbin "data/_chin_blue_chr4.bin"	; 1536 bytes

;_chin_blue_SG_arr:	
;	.word 1920, _chin_blue_chr0, bank(_chin_blue_chr0)
;	.word 1664, _chin_blue_chr1, bank(_chin_blue_chr1)
;	.word 1536, _chin_blue_chr2, bank(_chin_blue_chr2)
;	.word 1536, _chin_blue_chr3, bank(_chin_blue_chr3)
;	.word 1536, _chin_blue_chr4, bank(_chin_blue_chr4)


_chin_blue_palette:
_chin_blue_palette_slot0:
	.word $1C0, $8C, $CD, $155, $19D, $15E, $DB, $E0, $98, $EA, $1B5, $133, $5B, $1B6, $92, $00
_chin_blue_palette_end:

_chin_blue_num_frames:
	.byte $0A
_chin_blue_frames_data:
_chin_blue_idle01_RIGHT_frame:
	.word _chin_blue_idle01_RIGHT
	.byte bank(_chin_blue_idle01_RIGHT)
	.word _chin_blue_idle01_RIGHT_end - _chin_blue_idle01_RIGHT	; data size
	.byte 0		; GFX bank index (chr0)
_chin_blue_walk01_RIGHT_frame:
	.word _chin_blue_walk01_RIGHT
	.byte bank(_chin_blue_walk01_RIGHT)
	.word _chin_blue_walk01_RIGHT_end - _chin_blue_walk01_RIGHT
	.byte 1
_chin_blue_walk02_RIGHT_frame:
	.word _chin_blue_walk02_RIGHT
	.byte bank(_chin_blue_walk02_RIGHT)
	.word _chin_blue_walk02_RIGHT_end - _chin_blue_walk02_RIGHT
	.byte 2
_chin_blue_walk03_RIGHT_frame:
	.word _chin_blue_walk03_RIGHT
	.byte bank(_chin_blue_walk03_RIGHT)
	.word _chin_blue_walk03_RIGHT_end - _chin_blue_walk03_RIGHT
	.byte 3
_chin_blue_walk04_RIGHT_frame:
	.word _chin_blue_walk04_RIGHT
	.byte bank(_chin_blue_walk04_RIGHT)
	.word _chin_blue_walk04_RIGHT_end - _chin_blue_walk04_RIGHT
	.byte 4
_chin_blue_idle01_LEFT_frame:
	.word _chin_blue_idle01_LEFT
	.byte bank(_chin_blue_idle01_LEFT)
	.word _chin_blue_idle01_LEFT_end - _chin_blue_idle01_LEFT
	.byte 0
_chin_blue_walk01_LEFT_frame:
	.word _chin_blue_walk01_LEFT
	.byte bank(_chin_blue_walk01_LEFT)
	.word _chin_blue_walk01_LEFT_end - _chin_blue_walk01_LEFT
	.byte 1
_chin_blue_walk02_LEFT_frame:
	.word _chin_blue_walk02_LEFT
	.byte bank(_chin_blue_walk02_LEFT)
	.word _chin_blue_walk02_LEFT_end - _chin_blue_walk02_LEFT
	.byte 2
_chin_blue_walk03_LEFT_frame:
	.word _chin_blue_walk03_LEFT
	.byte bank(_chin_blue_walk03_LEFT)
	.word _chin_blue_walk03_LEFT_end - _chin_blue_walk03_LEFT
	.byte 3
_chin_blue_walk04_LEFT_frame:
	.word _chin_blue_walk04_LEFT
	.byte bank(_chin_blue_walk04_LEFT)
	.word _chin_blue_walk04_LEFT_end - _chin_blue_walk04_LEFT
	.byte 4


	; #1: Y pos, #2: X pos, #3: CHR index, #4: CHR desc

_chin_blue_idle01_RIGHT:
	.word $FFB0, $FFE9, $100, $82
	.word $FFB0, $FFF9, $102, $82
	.word $FFB0, $09, $104, $82
	.word $FFC0, $FFE9, $106, $82
	.word $FFC0, $FFF9, $108, $82
	.word $FFC0, $09, $10A, $82
	.word $FFD0, $FFE9, $10C, $82
	.word $FFD0, $FFF9, $10E, $82
	.word $FFD0, $09, $110, $82
	.word $FFE0, $FFE9, $112, $82
	.word $FFE0, $FFF9, $114, $82
	.word $FFE0, $09, $116, $82
	.word $FFF0, $FFE9, $118, $82
	.word $FFF0, $FFF9, $11A, $82
	.word $FFF0, $09, $11C, $82
_chin_blue_idle01_RIGHT_end:

_chin_blue_walk01_RIGHT:
	.word $FFB0, $FFEC, $100, $82
	.word $FFB0, $FFFC, $102, $82
	.word $FFC0, $FFEC, $104, $82
	.word $FFC0, $FFFC, $106, $82
	.word $FFC0, $0C, $108, $82
	.word $FFD0, $FFEC, $10A, $82
	.word $FFD0, $FFFC, $10C, $82
	.word $FFD0, $0C, $10E, $82
	.word $FFE0, $FFEC, $110, $82
	.word $FFE0, $FFFC, $112, $82
	.word $FFF0, $FFEC, $114, $82
	.word $FFF0, $FFFC, $116, $82
	.word $FFF0, $0C, $118, $82
_chin_blue_walk01_RIGHT_end:

_chin_blue_walk02_RIGHT:
	.word $FFB0, $FFEC, $100, $82
	.word $FFB0, $FFFC, $102, $82
	.word $FFC0, $FFEC, $104, $82
	.word $FFC0, $FFFC, $106, $82
	.word $FFC0, $0C, $108, $82
	.word $FFD0, $FFEC, $10A, $82
	.word $FFD0, $FFFC, $10C, $82
	.word $FFD0, $0C, $10E, $82
	.word $FFE0, $FFEC, $110, $82
	.word $FFE0, $FFFC, $112, $82
	.word $FFF0, $FFEC, $114, $82
	.word $FFF0, $FFFC, $116, $82
_chin_blue_walk02_RIGHT_end:

_chin_blue_walk03_RIGHT:
	.word $FFB0, $FFEC, $100, $82
	.word $FFB0, $FFFC, $102, $82
	.word $FFC0, $FFEC, $104, $82
	.word $FFC0, $FFFC, $106, $82
	.word $FFC0, $0C, $108, $82
	.word $FFD0, $FFEC, $10A, $82
	.word $FFD0, $FFFC, $10C, $82
	.word $FFD0, $0C, $10E, $82
	.word $FFE0, $FFEC, $110, $82
	.word $FFE0, $FFFC, $112, $82
	.word $FFF0, $FFEC, $114, $82
	.word $FFF0, $FFFC, $116, $82
_chin_blue_walk03_RIGHT_end:

_chin_blue_walk04_RIGHT:
	.word $FFB0, $FFEC, $100, $82
	.word $FFB0, $FFFC, $102, $82
	.word $FFC0, $FFEC, $104, $82
	.word $FFC0, $FFFC, $106, $82
	.word $FFC0, $0C, $108, $82
	.word $FFD0, $FFEC, $10A, $82
	.word $FFD0, $FFFC, $10C, $82
	.word $FFD0, $0C, $10E, $82
	.word $FFE0, $FFEC, $110, $82
	.word $FFE0, $FFFC, $112, $82
	.word $FFF0, $FFEC, $114, $82
	.word $FFF0, $FFFC, $116, $82
_chin_blue_walk04_RIGHT_end:

_chin_blue_idle01_LEFT:
	.word $FFB0, $07, $100, $882
	.word $FFB0, $FFF7, $102, $882
	.word $FFB0, $FFE7, $104, $882
	.word $FFC0, $07, $106, $882
	.word $FFC0, $FFF7, $108, $882
	.word $FFC0, $FFE7, $10A, $882
	.word $FFD0, $07, $10C, $882
	.word $FFD0, $FFF7, $10E, $882
	.word $FFD0, $FFE7, $110, $882
	.word $FFE0, $07, $112, $882
	.word $FFE0, $FFF7, $114, $882
	.word $FFE0, $FFE7, $116, $882
	.word $FFF0, $07, $118, $882
	.word $FFF0, $FFF7, $11A, $882
	.word $FFF0, $FFE7, $11C, $882
_chin_blue_idle01_LEFT_end:

_chin_blue_walk01_LEFT:
	.word $FFB0, $04, $100, $882
	.word $FFB0, $FFF4, $102, $882
	.word $FFC0, $04, $104, $882
	.word $FFC0, $FFF4, $106, $882
	.word $FFC0, $FFE4, $108, $882
	.word $FFD0, $04, $10A, $882
	.word $FFD0, $FFF4, $10C, $882
	.word $FFD0, $FFE4, $10E, $882
	.word $FFE0, $04, $110, $882
	.word $FFE0, $FFF4, $112, $882
	.word $FFF0, $04, $114, $882
	.word $FFF0, $FFF4, $116, $882
	.word $FFF0, $FFE4, $118, $882
_chin_blue_walk01_LEFT_end:

_chin_blue_walk02_LEFT:
	.word $FFB0, $04, $100, $882
	.word $FFB0, $FFF4, $102, $882
	.word $FFC0, $04, $104, $882
	.word $FFC0, $FFF4, $106, $882
	.word $FFC0, $FFE4, $108, $882
	.word $FFD0, $04, $10A, $882
	.word $FFD0, $FFF4, $10C, $882
	.word $FFD0, $FFE4, $10E, $882
	.word $FFE0, $04, $110, $882
	.word $FFE0, $FFF4, $112, $882
	.word $FFF0, $04, $114, $882
	.word $FFF0, $FFF4, $116, $882
_chin_blue_walk02_LEFT_end:

_chin_blue_walk03_LEFT:
	.word $FFB0, $04, $100, $882
	.word $FFB0, $FFF4, $102, $882
	.word $FFC0, $04, $104, $882
	.word $FFC0, $FFF4, $106, $882
	.word $FFC0, $FFE4, $108, $882
	.word $FFD0, $04, $10A, $882
	.word $FFD0, $FFF4, $10C, $882
	.word $FFD0, $FFE4, $10E, $882
	.word $FFE0, $04, $110, $882
	.word $FFE0, $FFF4, $112, $882
	.word $FFF0, $04, $114, $882
	.word $FFF0, $FFF4, $116, $882
_chin_blue_walk03_LEFT_end:

_chin_blue_walk04_LEFT:
	.word $FFB0, $04, $100, $882
	.word $FFB0, $FFF4, $102, $882
	.word $FFC0, $04, $104, $882
	.word $FFC0, $FFF4, $106, $882
	.word $FFC0, $FFE4, $108, $882
	.word $FFD0, $04, $10A, $882
	.word $FFD0, $FFF4, $10C, $882
	.word $FFD0, $FFE4, $10E, $882
	.word $FFE0, $04, $110, $882
	.word $FFE0, $FFF4, $112, $882
	.word $FFF0, $04, $114, $882
	.word $FFF0, $FFF4, $116, $882
_chin_blue_walk04_LEFT_end:

