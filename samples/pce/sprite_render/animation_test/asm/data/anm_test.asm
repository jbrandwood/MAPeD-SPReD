;#######################################################
;
; Generated by SPReD-PCE Copyright 2017-2022 0x8BitDev
;
;#######################################################

ANM_TEST_SPR_VADDR	= $2000

;anm_test_chr0:	.incbin "anm_test_chr0.bin"	; 9600 bytes

;anm_test_SG_arr:	
;	.word 9600, anm_test_chr0


anm_test_palette:
anm_test_palette_slot0:
	.word $147, $1FD, $16C, $1FE, $1B8, $130, $98, $17B, $132, $F0, $A8, $60, $1FF, $1B6, $DB, $00
anm_test_palette_end:

ANM_TEST_PALETTE_SLOT	= 0


anm_test_num_frames:
	.byte $06
anm_test_frames_data:
martha_punch01_frame:
	.word martha_punch01
	.byte bank(martha_punch01)
martha_punch02_frame:
	.word martha_punch02
	.byte bank(martha_punch02)
martha_punch03_frame:
	.word martha_punch03
	.byte bank(martha_punch03)
martha_punch04_frame:
	.word martha_punch04
	.byte bank(martha_punch04)
martha_punch05_frame:
	.word martha_punch05
	.byte bank(martha_punch05)
martha_punch06_ref04_frame:
	.word martha_punch06_ref04
	.byte bank(martha_punch06_ref04)


	; #1: Y pos, #2: X pos, #3: CHR index, #4: CHR desc

martha_punch01:
	.word martha_punch01_end - martha_punch01 - 3	; data size
	.byte 0		; GFX bank index (chr0)

	.word $FFA7, $FFF6, $100, $80
	.word $FFA7, $06, $102, $80
	.word $FFB7, $FFF2, $104, $80
	.word $FFB7, $02, $106, $80
	.word $FFB7, $12, $108, $80
	.word $FFC7, $FFF2, $10A, $80
	.word $FFC7, $02, $10C, $80
	.word $FFD7, $FFF2, $10E, $80
	.word $FFD7, $02, $110, $80
	.word $FFE7, $FFEA, $112, $80
	.word $FFE7, $FFFA, $114, $80
	.word $FFE7, $0A, $116, $80
	.word $FFF7, $FFE8, $118, $80
	.word $FFF7, $02, $11A, $80
martha_punch01_end:

martha_punch02:
	.word martha_punch02_end - martha_punch02 - 3	; data size
	.byte 0		; GFX bank index (chr0)

	.word $FFA7, $FFED, $11C, $80
	.word $FFA7, $FFFD, $11E, $80
	.word $FFB7, $FFE9, $120, $80
	.word $FFB7, $FFF9, $122, $80
	.word $FFB7, $09, $124, $80
	.word $FFC7, $FFEA, $126, $80
	.word $FFC7, $FFFA, $128, $80
	.word $FFC7, $0A, $12A, $80
	.word $FFD7, $FFEC, $12C, $80
	.word $FFD7, $FFFE, $12E, $80
	.word $FFE7, $FFEA, $130, $80
	.word $FFF7, $FFE8, $132, $80
martha_punch02_end:

martha_punch03:
	.word martha_punch03_end - martha_punch03 - 3	; data size
	.byte 0		; GFX bank index (chr0)

	.word $FFAD, $FFF4, $134, $80
	.word $FFAD, $04, $136, $80
	.word $FFAD, $14, $138, $80
	.word $FFBD, $FFF6, $13A, $80
	.word $FFBD, $06, $13C, $80
	.word $FFCD, $FFF2, $13E, $80
	.word $FFCD, $02, $140, $80
	.word $FFCD, $12, $142, $80
	.word $FFDD, $FFEC, $144, $80
	.word $FFDD, $FFFC, $146, $80
	.word $FFDD, $0C, $148, $80
	.word $FFED, $FFE8, $14A, $80
	.word $FFED, $03, $14C, $80
	.word $FFFD, $FFE8, $14E, $80
	.word $FFFD, $07, $150, $80
martha_punch03_end:

martha_punch04:
	.word martha_punch04_end - martha_punch04 - 3	; data size
	.byte 0		; GFX bank index (chr0)

	.word $FFA0, $0C, $152, $80
	.word $FFA0, $1C, $154, $80
	.word $FFB0, $FFF5, $156, $80
	.word $FFB0, $05, $158, $80
	.word $FFB0, $15, $15A, $80
	.word $FFB0, $25, $15C, $80
	.word $FFB0, $35, $15E, $80
	.word $FFC0, $FFFB, $160, $80
	.word $FFC0, $0B, $162, $80
	.word $FFC0, $1C, $164, $80
	.word $FFC0, $2C, $166, $80
	.word $FFD0, $FFF6, $168, $80
	.word $FFD0, $06, $16A, $80
	.word $FFD0, $16, $16C, $80
	.word $FFE0, $FFED, $16E, $80
	.word $FFE0, $FFFD, $170, $80
	.word $FFE0, $0D, $172, $80
	.word $FFF0, $FFE8, $174, $80
	.word $FFF0, $04, $176, $80
martha_punch04_end:

martha_punch05:
	.word martha_punch05_end - martha_punch05 - 3	; data size
	.byte 0		; GFX bank index (chr0)

	.word $FF90, $0E, $178, $80
	.word $FFA0, $01, $17A, $80
	.word $FFA0, $11, $17C, $80
	.word $FFB0, $FFF7, $17E, $80
	.word $FFB0, $07, $180, $80
	.word $FFB0, $17, $182, $80
	.word $FFC0, $FFF5, $184, $80
	.word $FFC0, $05, $186, $80
	.word $FFD0, $FFEE, $188, $80
	.word $FFD0, $FFFE, $18A, $80
	.word $FFE0, $FFED, $18C, $80
	.word $FFE0, $FFFD, $18E, $80
	.word $FFF0, $FFE8, $190, $80
	.word $FFF0, $04, $192, $80
	.word $00, $06, $194, $80
martha_punch05_end:

martha_punch06_ref04:
	.word martha_punch06_ref04_end - martha_punch06_ref04 - 3	; data size
	.byte 0		; GFX bank index (chr0)

	.word $FFA0, $0C, $152, $80
	.word $FFA0, $1C, $154, $80
	.word $FFB0, $FFF5, $156, $80
	.word $FFB0, $05, $158, $80
	.word $FFB0, $15, $15A, $80
	.word $FFB0, $25, $15C, $80
	.word $FFB0, $35, $15E, $80
	.word $FFC0, $FFFB, $160, $80
	.word $FFC0, $0B, $162, $80
	.word $FFC0, $1C, $164, $80
	.word $FFC0, $2C, $166, $80
	.word $FFD0, $FFF6, $168, $80
	.word $FFD0, $06, $16A, $80
	.word $FFD0, $16, $16C, $80
	.word $FFE0, $FFED, $16E, $80
	.word $FFE0, $FFFD, $170, $80
	.word $FFE0, $0D, $172, $80
	.word $FFF0, $FFE8, $174, $80
	.word $FFF0, $04, $176, $80
martha_punch06_ref04_end:

