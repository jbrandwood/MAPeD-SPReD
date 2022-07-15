;#######################################################
;
; Generated by SPReD-SMS Copyright 2017-2022 0x8BitDev
;
;#######################################################

.define	IMAGE_SPR_MODE_8X16	0
.define	IMAGE_SPR_CHR_BPP	4
.define	IMAGE_SPR_CHRS_OFFSET	0	; first CHR index in a CHR bank


;image_chr0:	.incbin "image_chr0.bin"	; 2048 bytes

;image_CHR_arr:	
;	.word 2048, image_chr0


image_palette:
	.byte $00, $03, $07, $0B, $35, $1E, $18, $1C, $22, $23, $29, $2D, $33, $36, $17, $3E


image_num_frames:
	.byte $01
image_frames_data:
bpp_test_sprite_frame:
	.word bpp_test_sprite


	; #1: Y pos, #2: X pos, #3: CHR index

bpp_test_sprite:
	.byte bpp_test_sprite_end - bpp_test_sprite - 2	; data size
	.byte 0		; GFX bank index (chr0)

	.byte $00, $00, $00
	.byte $00, $08, $01
	.byte $00, $10, $02
	.byte $00, $18, $03
	.byte $00, $20, $04
	.byte $00, $28, $05
	.byte $00, $30, $06
	.byte $00, $38, $07
	.byte $08, $00, $08
	.byte $08, $08, $09
	.byte $08, $10, $0A
	.byte $08, $18, $0B
	.byte $08, $20, $0C
	.byte $08, $28, $0D
	.byte $08, $30, $0E
	.byte $08, $38, $0F
	.byte $10, $00, $10
	.byte $10, $08, $11
	.byte $10, $10, $12
	.byte $10, $18, $13
	.byte $10, $20, $14
	.byte $10, $28, $15
	.byte $10, $30, $16
	.byte $10, $38, $17
	.byte $18, $00, $18
	.byte $18, $08, $19
	.byte $18, $10, $1A
	.byte $18, $18, $1B
	.byte $18, $20, $1C
	.byte $18, $28, $1D
	.byte $18, $30, $1E
	.byte $18, $38, $1F
	.byte $20, $00, $20
	.byte $20, $08, $21
	.byte $20, $10, $22
	.byte $20, $18, $23
	.byte $20, $20, $24
	.byte $20, $28, $25
	.byte $20, $30, $26
	.byte $20, $38, $27
	.byte $28, $00, $28
	.byte $28, $08, $29
	.byte $28, $10, $2A
	.byte $28, $18, $2B
	.byte $28, $20, $2C
	.byte $28, $28, $2D
	.byte $28, $30, $2E
	.byte $28, $38, $2F
	.byte $30, $00, $30
	.byte $30, $08, $31
	.byte $30, $10, $32
	.byte $30, $18, $33
	.byte $30, $20, $34
	.byte $30, $28, $35
	.byte $30, $30, $36
	.byte $30, $38, $37
	.byte $38, $00, $38
	.byte $38, $08, $39
	.byte $38, $10, $3A
	.byte $38, $18, $3B
	.byte $38, $20, $3C
	.byte $38, $28, $3D
	.byte $38, $30, $3E
	.byte $38, $38, $3F
bpp_test_sprite_end:

