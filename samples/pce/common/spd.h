//######################################################################################################
//
// This file is a part of the MAPeD-PCE Copyright 2017-2022 0x8BitDev ( MIT license. See LICENSE.txt )
// Desc: It contains a meta-sprite rendering routines using a local HuC's SATB
//
//######################################################################################################

/*/	SPD-render v0.6
History:

v0.6
2022.07.11 - added 'SPD_SG_BANK_INIT_VAL' as initial value for the '_last_bank_ind' in 'spd_sprite_params'
2022.07.11 - extremely simplified double-buffering logic for meta-sprites
2022.07.11 - the double-buffer index in 'spd_get_dbl_buff_ind' and the second argument in 'spd_dbl_buff_VRAM_addr' changed to a byte value
2022.07.08 - added 'spd_SATB_push_simple_sprite' for simple sprites, it takes a little less processing time compared to 'spd_SATB_push_sprite' and allows you to use sprite offset values unlike HuC sprite functions
2022.07.08 - the type of the second argument for 'spd_copy_SG_data_to_VRAM' and 'spd_SG_data_params' has changed, now '_src_bank' is a byte and therefore a pointer to a byte
2022.07.08 - changed a sprite index register __dl -> __bh in 'spd_SATB_push_sprite' and 'spd_SATB_push_simple_sprite' for using HuC rand() as argument for X, Y

v0.5
2022.07.03 - asm routines are wrapped in .proc/.endp
2022.07.03 - added 'spd_change_palette( _plt_ind )' function to change palette after the 'spd_SATB_push_sprite' function call
2022.06.28 - added 'spd_alt_VRAM_addr( _alt_VADDR )' function to use VRAM address other than exported one
2022.06.25 - optimized loading of SG data to VRAM for double-buffered meta-sprites; added 'SPD_DBL_BUFF_INIT_VAL' as initial value for a double-buffer index
2022.06.25 - small changes in 'SPD_DEBUG', the white border now shows how long the 'spd_SATB_push_sprite' takes
2022.06.23 - added second argument to the 'spd_dbl_buff_VRAM_addr( VADDR_dbl_buff, _dbl_buff_ind )' function and added 'spd_get_dbl_buff_ind()' function
2022.06.09 - small fix in 'SPD_DEBUG', the pink border now shows how long it takes to load graphics data to VRAM

v0.4
2022.06.08 - added debug flag 'SPD_DEBUG' that shows when gfx data is being loaded to VRAM (use Mednafen)
2022.06.08 - added flag 'SPD_SG_NEW_DATA' to the 'spd_SATB_push_sprite' function result
2022.06.07 - added fourth argument to the 'spd_sprite_params( <exported_name>_SG_arr, <EXPORTED_NAME>_SPR_VADDR, _flag, _last_bank_ind )' and also added 'spd_SG_bank_get_ind()'
2022.06.07 - changed 'spd_copy_SG_data_to_VRAM( _SG_ind )' to 'spd_copy_SG_data_to_VRAM( <exported_name>_frames_data, _spr_ind )' and added 'spd_copy_SG_data_to_VRAM( <animation_name>_frame )'
2022.06.04 - changed exported data, now '<exported_name>_PALETTE_SLOT' includes sprite palette offset (16) and '<exported_name>_palette_size' is the number of active palettes
2022.06.02 - fixed _spd_farptr_add_offset

v0.3
2022.05.03 - added 'spd_copy_SG_to_VRAM( _SG_ind )'

v0.2
2022.04.24 - optimization of '__attr_loop_XY', '__attr_loop_XY_IND' loops
2022.04.22 - added 'SPD_FLAG_IGNORE_SG'

v0.1
2022.04.22 - initial release

~~~~~~~~~~
debug info (use Mednafen):
 - pink border color - ROM-VRAM data copying
 - white border color - spd_SATB_push_sprite
 
#asm
SPD_DEBUG
#endasm

NOTE:	The SPReD-PCE exports both meta-sprites and simple sprites (16x16,16x32,16x64,32x16,32x32,32x64). The CGX/CGY
	flags are automatically applied to exported sprites. So you don't need to configure anything in your HuC program.

*SG - sprite graphics data

The SPD sprite/meta-sprite render works as an extension to the HuC local SATB. So you can combine it with the HuC`s sprite functions.
There are two data types the SPD-render works with.

1. PACKED sprites data. All exported SG data are stored in a single file. It were packed in the SPReD-PCE before exporting.
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
The main logic is:

	// SPD-render initialization.
	spd_init();	

	// Initialization of exported sprite set.
	{
		// Load palette in the usual way.
		load_palette( <EXPORTED_NAME>_PALETTE_SLOT, <exported_name>_palette, <exported_name>_PALETTE_SIZE );

		// Set up exported sprite set with SG data array and VRAM address to load SG data to.
		// NOTE: You can combine any number of exported sprite sets in your program.
		//	 Call the 'spd_sprite_params' to switch between them.
[upd] v0.5	// NOTE: Passing ZERO as the third parameter, means that SG data will be automatically
--->		//	 loaded to VRAM on 'spd_SATB_push_sprite'.
[upd] v0.2	// NOTE: Passing 'SPD_FLAG_IGNORE_SG' as the third parameter will ignore loading SG to VRAM.
		//	 It's useful for PACKED(!) sprites when you are switching to a sprite set and SG data already loaded to VRAM.
--->		//	 Such way you avoid loading SG to VRAM twice.
[upd] v0.4	// NOTE: Passing 'last_bank_ind' allows to avoid loading SG data to VRAM twice when you are switching back from another data set.
[upd] v0.6	//	 The last value can be obtained using 'spd_SG_bank_get_ind()'. The initial value is 'SPD_SG_BANK_INIT_VAL'.
--->		spd_sprite_params( <exported_name>_SG_arr, <EXPORTED_NAME>_SPR_VADDR, 0, last_bank_ind );

[upd] v0.3	// NOTE: There are two ways to load SG data to VRAM:
		//	 1. Indirect loading, when you push the first sprite by calling 'spd_SATB_push_sprite'.
		//	 The third argument for the 'spd_sprite_params' must be ZERO.
--->		//
[upd] v0.4	//	 2. Direct loading, when you call 'spd_copy_SG_data_to_VRAM' with a sprite data frame/index.
		//	 The third argument for the 'spd_sprite_params' must be 'SPD_FLAG_IGNORE_SG'.
		//
		//	 spd_copy_SG_data_to_VRAM( <exported_name>_frames_data, _spr_ind )
--->		//	 spd_copy_SG_data_to_VRAM( <animation_name>_frame )
	}

	// HuC's SATB initialization.
	init_satb();

	// Here you can use HuC's sprite calls...

	// SPD calls
	{
		// Now you can set a local SATB position to push your exported sprites/meta-sprites to.
		// NOTE: The SATB position will be automatically incremented with each call to 'spd_SATB_push_sprite' (!)
		spd_SATB_set_pos( <SATB_pos[0...63]> );
[upd] v0.6
		// NOTE: There are two ways to push sprite to SATB:
		//	 1. spd_SATB_push_sprite - for meta-sprites
		//
		//	 2. spd_SATB_push_simple_sprite - for simple sprites, it takes a little less processing time
		//	 compared to 'spd_SATB_push_sprite' and allows you to use sprite offset values, that will be
		//	 zeroed out when using HuC sprite functions.
		//
		// NOTE: Double-buffering and 'spd_change_palette' aren't supported with 'spd_SATB_push_simple_sprite'.
--->
		// There are two ways to show a sprite:
		// 1. By index in a sprite array. Suitable for animation sequences.
		// (see exported .h file for generated constants)
		spd_SATB_push_sprite( <exported_name>_frames_data, _ind, _x, _y );
		OR
[upd] v0.6	spd_SATB_push_simple_sprite( <exported_name>_frames_data, _ind, _x, _y );

		// 2. By sprite data pointer.
		// (see exported .h file for generated data)
		spd_SATB_push_sprite( <animation_name>_frame, _x, _y );
		OR
[upd] v0.6	spd_SATB_push_simple_sprite( <animation_name>_frame, _x, _y );

[upd] v0.3	// NOTE: SG data will be automatically loaded once to VRAM at first call to the 'spd_SATB_push_sprite',
--->		//	 when the third parameter passed to the 'spd_sprite_params' is ZERO (!)
		// NOTE: If meta-sprite does not fit into SATB, it will be ignored!
[upd] v0.4	// NOTE: 'spd_SATB_push_sprite' returns: 1-Ok + ORed flag 'SPD_SG_NEW_DATA' when a new SG data already or must be loaded to VRAM; 0-SATB overflow

		// Save SG bank index, especially if you are using more than one sprite set(!)
--->		last_bank_ind	= spd_SG_bank_get_ind();
	}

	// Then call 'satb_update' to push your sprite data to VRAM SAT.
	// NOTE: After pushing all sprites, `spd_SATB_get_pos()` returns the number of sprites in SATB.
	satb_update( spd_SATB_get_pos() );


	// NOTE: As mentioned before, you can combine the SPD calls with the HuC ones.
	//	 For example, you can do the following for simple static sprites:
[upd] v0.3
	// Initialization at startup
	load_palette( ...
	spd_sprite_params( ...

	init_satb();
	spd_SATB_set_pos( 0 );

	spd_SATB_push_sprite( my_sprite_16x32, init_x, init_y );

	...

	// In update loop
	spr_set( 0 );
	spr_x( new_x );
	spr_y( new_y );

--->	// I recommend using simple static sprites this way, as it simplifies their initialization and is optimal for runtime.
	// WARNING: Take into account your sprite origin! It can be configured out of (0, 0) in the SPReD-PCE.


[upd] v0.5
	NOTE: If you need a cache of sprites, for example, for identical objects that may appear on screen in a certain quantity, 
	initialize and use the same set of sprites as many times as you need for your cache. In this case, all of your
--->	cached sprites will use the same graphics, but may be located at different positions on the screen.


2. UNPACKED sprites data. All exported SG data are stored in separate files. It were not packed in the SPReD-PCE.
Unpacked data are suitable for dynamic SG data, that can be loaded to VRAM dynamically to save video memory. It`s
useful when you have a lot of animations that don`t fit into VRAM, like in fighting games.
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
The main logic is:

	// SPD-render initialization.
	spd_init();	

	// Initialization of exported sprite set.
	{
		// Load palette in the usual way.
		load_palette( <EXPORTED_NAME>_PALETTE_SLOT, <exported_name>_palette, <exported_name>_PALETTE_SIZE );

		// Set up exported sprite set with SG data array and VRAM address to load SG data to.
#if	DEF_SG_DBL_BUFF
		// NOTE: Using the `SPD_FLAG_DBL_BUFF` flag means double-buffering for sprite graphics.
		//	 It costs x2 of dynamic SG data in VRAM, but glitches free. You have to compare the results
		//	 of using 'SPD_FLAG_DBL_BUFF' and 'SPD_FLAG_PEND_SG_DATA' and decide which is better in your case.
[upd] v0.4	// NOTE: Passing 'last_bank_ind' allows to avoid loading SG data to VRAM twice when you are switching back from another data set.
[upd] v0.6	//	 The last value can be obtained using 'spd_SG_bank_get_ind()'. The initial value is 'SPD_SG_BANK_INIT_VAL'.
--->		spd_sprite_params( <exported_name>_SG_arr, <EXPORTED_NAME>_SPR_VADDR, SPD_FLAG_DBL_BUFF, last_bank_ind );

[upd] v0.5	// Set the second VRAM address for double-buffering (SPD_FLAG_DBL_BUFF) and the last double-buffer index value (initial value is 'SPD_DBL_BUFF_INIT_VAL').
--->		spd_dbl_buff_VRAM_addr( VADDR_dbl_buff, last_dbl_buff_ind );
#else
		// NOTE: Using the `SPD_FLAG_PEND_SG_DATA` flag means that SG data will not be loaded
		//	 to VRAM automatically. You should do that manually on VBLANK.
		spd_sprite_params( <exported_name>_SG_arr, <EXPORTED_NAME>_SPR_VADDR, SPD_FLAG_PEND_SG_DATA, last_bank_ind );

		// Set pointers to SG data for delayed use (SPD_FLAG_PEND_SG_DATA).
		spd_SG_data_params( &SG_DATA_SRC_ADDR, &SG_DATA_SRC_BANK, &SG_DATA_DST_ADDR, &SG_DATA_LEN );
#endif
		// NOTE: Single- and double-buffered meta-sprites can be combined in runtime.
		// NOTE: You can combine any number of exported sprite sets in your program.
		//	 Call the 'spd_sprite_params' to switch between them.

[upd] v0.5	// NOTE: To use multiple instances of the same sprite set, for example, for a sprite cache,
--->		//	 use 'spd_alt_VRAM_addr( _alt_VADDR )' which replaces the '<EXPORTED_NAME>_SPR_VADDR'.
	}

	// HuC's SATB initialization.
	init_satb();

	// Here you can use HuC's sprite calls...

	// SPD calls
	{
		// Now you can set a local SATB position to push your exported sprites/meta-sprites to.
		// NOTE: The SATB position will be automatically incremented with each call to 'spd_SATB_push_sprite' (!)
		spd_SATB_set_pos( <SATB_pos[0...63]> );

		// There are two ways to show a sprite:
		// 1. By index in a sprite array. Suitable for animation sequences.
		// (see exported .h file for generated constants)
[upd] v0.5	// NOTE: Use this function ONLY, if double-buffering is enabled (!)
		spd_SATB_push_sprite( <exported_name>_frames_data, _ind, _x, _y );

		// 2. By sprite data pointer.
		// (see exported .h file for generated data)
[upd] v0.5	// NOTE: Don't use this function, if double-buffering is enabled (!)
		spd_SATB_push_sprite( <animation_name>_frame, _x, _y );

		// NOTE: If meta-sprite does not fit into SATB, it will be ignored!
[upd] v0.4	// NOTE: 'spd_SATB_push_sprite' returns: 1-Ok + ORed flag 'SPD_SG_NEW_DATA' when a new SG data already or must be loaded to VRAM; 0-SATB overflow

		// Save SG bank index, especially if you are using more than one sprite set(!)
--->		last_bank_ind	= spd_SG_bank_get_ind();
[upd] v0.5
#if		DEF_SG_DBL_BUFF
		last_dbl_buff_ind	= spd_get_dbl_buff_ind();
#endif
--->
	}

	// 'VRAM-SATB Transfer Auto-Repeat on VBLANK' (DCR ROF-$10) is enabled by default in HuC at startup, so we skip this step.

	// Main loop
	for( ;; )
	{
		// Update your sprite animation
		update_frame( &test_anim );

		// Here you can call the
		reset_satb();// to clear all the SATB data
		OR
		spd_SATB_clear_from( <SATB_pos[0...63]> );// to save sprites before 'SATB_pos', and clear memory after to avoid graphical glitches with variable sized meta-sprites

		// Set the SATB position to push your sprite to.
		spd_SATB_set_pos( <SATB_pos[0...63]> );

		// Push your sprite to the local RAM SATB.
[upd] v0.4	res_byte = spd_SATB_push_sprite( <exported_name>_frames_data, test_anim.start_frame + test_anim.curr_frame, _x, _y );

--->		last_bank_ind	= spd_SG_bank_get_ind();

[upd] v0.5	// NOTE: To change sprite palette, use 'spd_change_palette( _plt_ind )' function immediately after calling the 'spd_SATB_push_sprite',
		//	 if the result isn't zero. See './samples/pce/sprite_render/animation_test/huc3' and '/huc4' for details.
		if( res_byte )
		{
			spd_change_palette( _plt_ind );	// _plt_ind - 0...15
		}

#if		DEF_SG_DBL_BUFF
		last_dbl_buff_ind	= spd_get_dbl_buff_ind();
#endif
--->
		// Load all sprites to VRAM SAT.
		satb_update( spd_SATB_get_pos() );

		vsync();

#if	!DEF_SG_DBL_BUFF
		// The HuC doesn't allow to handle VBLANK directly, so we use 'vsync' instead for synchronization of sprite graphics and the inner SATB.
		// This may cause some graphical glitches at the upper part of the screen.

		// Delayed copying of SG data to VRAM to synchronize it with the inner SATB
[upd] v0.4	if( res_byte & SPD_SG_NEW_DATA )
		{
			spd_copy_SG_data_to_VRAM( SG_DATA_SRC_ADDR, SG_DATA_SRC_BANK, SG_DATA_DST_ADDR, SG_DATA_LEN );
		}
#endif
	}

[upd] v0.4	
3. Also you can use PACKED and UNPACKED data in one data set (in one SPReD-PCE project) by combining the approaches described above.

--->
	That`s it! :)
/*/

const unsigned char spd_ver[] = { "S", "P", "D", "0", "6", 0 };

/* SPD flag(s) */

// Copies SG data parameters: src_addr/src_bank/vram_addr/len for delayed use on VBLANK;
// it's suitable for sprites with dynamic SG data like in fighting games.
// NOTE: THIS FLAG CAN BE USED WITH UNPACKED SPRITES ONLY! WHERE EACH SPRITE HAS A SEPARATE SG DATA!
#define	SPD_FLAG_PEND_SG_DATA	0x01

// Double-buffering. It costs x2 of dynamic SG data in VRAM, but glitches free.
// You have to compare the results of using 'SPD_FLAG_DBL_BUFF' and 'SPD_FLAG_PEND_SG_DATA' and decide which is better for you.
// Thanks to elmer/pcengine.proboards.com for suggesting this mode.
// NOTE: THIS FLAG CAN BE USED WITH UNPACKED SPRITES ONLY! WHERE EACH SPRITE HAS A SEPARATE SG DATA!
#define	SPD_FLAG_DBL_BUFF	0x02

// Loading sprite graphics to VRAM will be ignored. It's useful for PACKED(!) sprites when you are switching to a sprite set and SG data already loaded to VRAM.
// Such way you avoid loading SG to VRAM twice.
#define	SPD_FLAG_IGNORE_SG	0x04

// ORed value returned by 'spd_SATB_push_sprite', means that new SG data already or must be loaded to VRAM
#define SPD_SG_NEW_DATA		0x80

// Initial value of a last double-buffer index
#define	SPD_DBL_BUFF_INIT_VAL	0x00

// Initial value for a SG data
#define	SPD_SG_BANK_INIT_VAL	0xff

/* main SPD-render routines */

void		__fastcall spd_init();
void		__fastcall spd_sprite_params( unsigned short far* _SG_data<__bl:__si>, unsigned short _VADDR<__dx>, unsigned char _flags<__al>, unsigned char _last_SG_bank_ind<__ah> );
unsigned char	__fastcall spd_SG_bank_get_ind();

void		__fastcall spd_SATB_set_pos( unsigned char _pos<__al> );// _pos: 0-63
unsigned char	__fastcall spd_SATB_get_pos();
void		__fastcall spd_SATB_clear_from( unsigned char _pos<__al> );// _pos: 0-63

unsigned char	__fastcall spd_SATB_push_sprite( unsigned char far* _frames_data<__bl:__si>, unsigned char _spr_ind<__bh>, unsigned short _x<__ax>, unsigned short _y<__cx> );// OUT: 1-Ok!, 0-SATB overflow
unsigned char	__fastcall spd_SATB_push_sprite( unsigned char far* _frame_addr<__bl:__si>, unsigned short _x<__ax>, unsigned short _y<__cx> );// OUT: 1-Ok!, 0-SATB overflow
unsigned char	__fastcall spd_SATB_push_simple_sprite( unsigned char far* _frames_data<__bl:__si>, unsigned char _spr_ind<__bh>, unsigned short _x<__ax>, unsigned short _y<__cx> );// OUT: 1-Ok!, 0-SATB overflow
unsigned char	__fastcall spd_SATB_push_simple_sprite( unsigned char far* _frame_addr<__bl:__si>, unsigned short _x<__ax>, unsigned short _y<__cx> );// OUT: 1-Ok!, 0-SATB overflow
void		__fastcall spd_change_palette( unsigned char _plt_ind<__al> );

void		__fastcall spd_SG_data_params( unsigned short _src_addr<__ax>, unsigned short _src_bank<__bx>, unsigned short _dst_addr<__cx>, unsigned short _len<__dx> );
void		__fastcall spd_copy_SG_data_to_VRAM( unsigned short _src_addr<__ax>, unsigned char _src_bank<__bl>, unsigned short _dst_addr<__dx>, unsigned short _len<__cx> );
void		__fastcall spd_copy_SG_data_to_VRAM( unsigned char far* _frames_data<__bl:__si>, unsigned char _spr_ind<__bh> );
void		__fastcall spd_copy_SG_data_to_VRAM( unsigned char far* _frame_addr<__bl:__si> );

void		__fastcall spd_dbl_buff_VRAM_addr( unsigned short _VADDR<__ax>, unsigned short _dbl_buff_ind<__bl> );
void		__fastcall spd_alt_VRAM_addr( unsigned short _VADDR<__ax> );
unsigned char	__fastcall spd_get_dbl_buff_ind();

#asm

; macros(es)

; val -> &((byte)(*zp))
	.macro stb_zpii ; \1 - val, \2 - zp
	cly
	lda low_byte \1
	sta [\2], y
	.endm

; val -> &((word)(*zp))
	.macro stw_zpii ; \1 - val, \2 - zp
	cly
	lda low_byte \1
	sta [\2], y
	iny
	lda high_byte \1
	sta [\2], y
	.endm

; (word(*zp)) -> addr
	.macro stw_zpii_rev ; \1 - zp, \2 - addr
	lda [<\1], y
	sta \2
	iny
	lda [<\1], y
	sta \2 + 1
	.endm

; \1 *= 2
	.macro mul2_word
	lda low_byte \1
	asl a	
	rol high_byte \1
	sta low_byte \1
	.endm	

; \1 *= 4
	.macro mul4_word
	lda low_byte \1
	asl a	
	rol high_byte \1
	asl a	
	rol high_byte \1
	sta low_byte \1
	.endm

; \1 *= 6
	.macro mul6_word
	; XY = 2x
	lda low_byte \1
	asl a
	tay
	rol high_byte \1
	ldx high_byte \1

	; \1 = 4x
	asl a
	rol high_byte \1
	sta low_byte \1

	; \1 = XY + 4x
	clc
	tya
	adc low_byte \1
	sta low_byte \1
	txa
	adc high_byte \1
	sta high_byte \1
	.endm

; \1 = bhdh * 6
	.macro mul6_bhdh
	; XY = 2x
	lda <__bh
	asl a
	tay
	rol <__dh
	ldx <__dh

	; \1 = 4x
	asl a
	rol <__dh
	sta <__bh

	; \1 = XY + 4x
	clc
	tya
	adc <__bh
	sta low_byte \1
	txa
	adc <__dh
	sta high_byte \1
	.endm

; \1 *= 8
	.macro mul8_word
	lda low_byte \1
	asl a	
	rol high_byte \1
	asl a	
	rol high_byte \1
	asl a	
	rol high_byte \1
	sta low_byte \1
	.endm	

; \1 /= 2
	.macro div2_word
	lda low_byte \1
	lsr high_byte \1
	ror a
	sta low_byte \1
	.endm

; \1 /= 8
	.macro div8_word
	lda low_byte \1
	lsr high_byte \1
	ror a
	lsr high_byte \1
	ror a
	lsr high_byte \1
	ror a
	sta low_byte \1
	.endm

; \1 /= 32
	.macro div32_word
	lda low_byte \1
	lsr high_byte \1
	ror a
	lsr high_byte \1
	ror a
	lsr high_byte \1
	ror a
	lsr high_byte \1
	ror a
	lsr high_byte \1
	ror a
	sta low_byte \1
	.endm

; -\1 /= 32
	.macro div32_neg_word
	lda low_byte \1
	sec
	ror high_byte \1
	ror a
	sec
	ror high_byte \1
	ror a
	sec
	ror high_byte \1
	ror a
	sec
	ror high_byte \1
	ror a
	sec
	ror high_byte \1
	ror a
	sta low_byte \1
	.endm

; \2 = \1 + \2
	.macro add_word_to_word
	clc
	lda low_byte \1
	adc low_byte \2
	sta low_byte \2
	lda high_byte \1
	adc high_byte \2
	sta high_byte \2
	.endm

; \1 += A
	.macro add_a_to_word
	clc
	adc low_byte \1
	sta low_byte \1

	cla
	adc high_byte \1
	sta high_byte \1
	.endm

; \2 = \1 - \2
	.macro sub_word_from_word
	lda low_byte \1
	sec
	sbc low_byte \2
	sta low_byte \2
	lda high_byte \1
	sbc high_byte \2
	sta high_byte \2
	.endm

	.macro get_SATB_flag
	lda <__SATB_flags
	and #\1
	.endm

	.macro inner_flags_dbl_buff_state
	lda <__inner_flags
	and #INNER_FLAGS_DBL_BUFF
	.endm

	.macro inner_flags_switch_dbl_buff
	lda <__inner_flags
	eor #INNER_FLAGS_DBL_BUFF
	sta <__inner_flags
	.endm

	.zp

SPD_FLAG_PEND_SG_DATA	= $01
SPD_FLAG_DBL_BUFF	= $02
SPD_FLAG_IGNORE_SG	= $04
SPD_FLAG_ALT_VADDR	= $08	; is used when the 'spd_alt_VRAM_addr()' is called

SPD_SG_NEW_DATA		= $80	; 'spd_SATB_push_sprite' result

SATB_SIZE	= 64

__SATB	= satb	; satb - HuC`s local satb

__SATB_pos	.ds 1
__SATB_flags	.ds 1
__last_SG_bank	.ds 1

INNER_FLAGS_DBL_BUFF	= %00000001

__inner_flags	.ds 1

; *** sprite data ***

__spr_VADDR	.ds 2	; sprite`s SG data address in VRAM
__spr_VADDR_dbl	.ds 2	; VRAM address for double-buffering
__spr_dbf_SG_offset
		.ds 2	; SG offset for pattern index correction when double-buffering is active

__spr_alt_SG_offset
		.ds 2	; SG offset for pattern index correction for alternative VADDR

__spr_SG_data_addr
		.ds 2
__spr_SG_data_bank
		.ds 1

__spr_pos_x:	.ds 2
__spr_pos_y:	.ds 2

	.bss

__SG_DATA_SRC_ADDR	.ds 2	; pointers
__SG_DATA_SRC_BANK	.ds 2	; to
__SG_DATA_DST_ADDR	.ds 2	; appropriate
__SG_DATA_LEN		.ds 2	; data

__TII:		.ds 1	; $73 tii
__bsrci:	.ds 2
__bdsti:	.ds 2
__bleni:	.ds 2
__tiirts	.ds 1	; $60 rts

	.code

;// spd_init()
;
	.proc _spd_init

	cla
	sta <__inner_flags

	; init TII

	lda #$73
	sta __TII
	lda #$60
	sta __tiirts

	rts

	.endp

;// spd_sprite_params( farptr __bl:__si / SG_data, word __dx / vaddr, byte __al / flags, byte __ah / last_SG_bank_ind )
;
	.proc _spd_sprite_params.4

	stw <__si, <__spr_SG_data_addr
	lda <__bl
	sta <__spr_SG_data_bank

	stw <__dx, <__spr_VADDR

	lda <__al
	sta <__SATB_flags

	lda <__ah
	sta <__last_SG_bank

	rts

	.endp

;// spd_SG_bank_get_ind()
;
	.proc _spd_SG_bank_get_ind

	ldx <__last_SG_bank
	cla

	rts

	.endp

	.procgroup

;// spd_dbl_buff_VRAM_addr( word __ax / vram_addr, word __bl / dbl_buff_ind )
;
	.proc _spd_dbl_buff_VRAM_addr.2

	; set double-buffer index

	lda <__bl
	beq .reset

	; set

	ora <__inner_flags
	sta <__inner_flags

	bra .cont

.reset:

	lda <__inner_flags
	and #(~INNER_FLAGS_DBL_BUFF)
	sta <__inner_flags

.cont:
	; set alternative VADDR

	stw <__ax, <__spr_VADDR_dbl
	stw <__spr_VADDR, <__bx

	call _calc_SG_pattern_offset

	stw <__bx, <__spr_dbf_SG_offset

	rts

	.endp

;// void spd_alt_VRAM_addr( word __ax / vram_addr )
;
	.proc _spd_alt_VRAM_addr.1

	; apply alt VADDR flag

	lda #SPD_FLAG_ALT_VADDR
	ora <__SATB_flags
	sta <__SATB_flags

	; set alternative VADDR

	stw <__spr_VADDR, <__bx
	stw <__ax, <__spr_VADDR			; replace the sprite VADDR with the alternative one

	call _calc_SG_pattern_offset

	stw <__bx, <__spr_alt_SG_offset

	rts

	.endp

;
; IN:	__ax - VADDR1
;	__bx - VADDR2
;
; OUT:	__bx - pattern index offset
;
	.proc _calc_SG_pattern_offset

	sub_word_from_word <__ax, <__bx

	lda <__bh
	bpl .pos_val

	; negative val

	; SG_offset = __bx / 32

	div32_neg_word <__bx

	rts

	; positive val

.pos_val:

	; SG_offset = __bx / 32

	div32_word <__bx

	rts

	.endp

	.endprocgroup

;// unsigned char spd_get_dbl_buff_ind()
;
	.proc _spd_get_dbl_buff_ind

	inner_flags_dbl_buff_state
	tax
	cla
	
	rts

	.endp

;// spd_SATB_set_pos( byte __al / pos )
;
	.proc _spd_SATB_set_pos.1

	lda <__al
	and #SATB_SIZE - 1		; clamp to 0-63
	sta <__SATB_pos

	rts

	.endp

	.proc _spd_SATB_get_pos

	ldx <__SATB_pos
	cla

	rts

	.endp

;// spd_SATB_clear_from( byte __al / pos )
;
	.proc _spd_SATB_clear_from.1

	lda <__al
	and #SATB_SIZE - 1		; clamp to 0-63
	sta __bsrci
	tax

	; __bsrci

	stz __bsrci + 1
	mul8_word __bsrci

	add_word_to_word #__SATB, __bsrci

	; clear the first byte

	stw __bsrci, <__ax
	cly
	cla
	sta [<__ax], y

	; __bdsti

	stw __bsrci, __bdsti

	; ++__bdsti

	inc __bdsti
	bne .cont1
	inc __bdsti + 1
.cont1:

	; __bleni

	txa
	sec
	sbc #SATB_SIZE
	eor #$ff
	inc a

	sta __bleni
	stz __bleni + 1

	mul8_word __bleni

	; --__bleni

	dec __bleni
	lda __bleni
	eor #$ff
	bne .cont2
	dec __bleni + 1
.cont2:

	jmp __TII

	.endp

;// spd_change_palette( byte __al / plt_ind )
;
	.proc _spd_change_palette.1

	lda <__al
	and #%00001111			; clamp 16 colors area
	ora #%10000000			; set SPBG bit by default
	tax

	stw __bleni, <__bx
	div8_word <__bx			; __bl - number of sprites

	stw __bdsti, <__cx
	lda #6				; offset to the first palette byte
	add_a_to_word <__cx

	cly
.loop:
	txa
	sta [<__cx], y

	clc
	tya
	adc #8
	tay

	bne .cont

	inc <__ch
.cont:
	dec <__bl
	bne .loop

	rts

	.endp

	.procgroup

; *** farptr += offset ***
;
; IN:
;
; __dx - offset
; __bl - bank number
; __si - address
;
	.proc _spd_farptr_add_offset

	; add an offset

	clc
	lda <__dx
	adc <__si
	sta <__si
	lda <__si+1
	and #$1f
	adc <__dx+1

	tay

	; increment a bank number

	lsr a
	lsr a
	lsr a
	lsr a
	lsr a
	clc
	adc <__bl
	sta <__bl

	; save high byte of a bank address

	tya
	and #$1f
	ora #$60
	sta <__si+1	
	
	rts

	.endp

;// spd_SATB_push_sprite( farptr __bl:__si / addr, byte __bh / index, word __ax / x_pos, word __cx / y_pos )
;
	.proc _spd_SATB_push_sprite.4

.ifdef	SPD_DEBUG
	call _push_sprite_border
.endif

	; offset x6 -> sizeof( spd_SPRITE )

	stz <__dh
	mul6_bhdh <__dx

	call _spd_farptr_add_offset

	call _spd_SATB_push_sprite.3

	rts

	.endp

;// spd_SATB_push_sprite( farptr __bl:__si / addr, word __ax / x_pos, word __cx / y_pos )
;
	.proc _spd_SATB_push_sprite.3

.ifdef	SPD_DEBUG
	call _push_sprite_border
.endif

	; XY coordinates correction

	lda #32
	clc
	adc <__ax
	sta <__spr_pos_x
	cla
	adc <__ax + 1
	sta <__spr_pos_x + 1

	lda #64
	clc
	adc <__cx
	sta <__spr_pos_y
	cla
	adc <__cx + 1
	sta <__spr_pos_y + 1

	jsr map_data			; map spd_SPRITE data

	; get meta-sprite address and bank

	cly				; meta-sprite address offset
	stw_zpii_rev __si, <__cx

	ldy #$02			; meta-sprite bank offset
	lda [<__si], y
	sta <__al			; __al - bank

	; get data length

	iny				; meta-sprite length offset
	stw_zpii_rev __si, __bleni

	iny				; meta-sprite SG bank offset
	lda [<__si], y
	tay				; Y - SG bank index

	jsr unmap_data

	stw <__cx, <__si		; matasprite address
	lda <__al
	sta <__bl			; meta-sprite bank

	; check SATB overflow

	stw __bleni, <__ax
	div8_word <__ax			; __al - number of sprites
	clc
	adc <__SATB_pos
	cmp #SATB_SIZE + 1
	bcc .push_sprite

	; SATB overflow

.ifdef	SPD_DEBUG
	call _black_border
.endif

	clx
	cla

	rts

.push_sprite:

	jsr map_data			; map meta-sprite data

	; _bsrci = meta-sprite address

	stw <__si, __bsrci

	; calc SATB address to copy sprite data to
	; _bdsti = __SATB + ( __SATB_pos * 8 )

	lda <__SATB_pos
	sta <__cl
	stz <__ch
	mul8_word <__cx			; __cx - SATB offset in bytes

	add_word_to_word #__SATB, <__cx
	stw <__cx, __bdsti

	; copy meta-sprite data to the local SATB

	jsr __TII

	jsr unmap_data

	phy				; Y - SG bank index

;--- DBL-BUFF ---
	; check double-buffering state

	get_SATB_flag SPD_FLAG_DBL_BUFF
	beq .use_main_buff		; no double-buffering

	; double-buffering is enabled

	cpy <__last_SG_bank
	beq .skip_dbl_buff_switch	; we have the same SG bank data, so skip double-buffer switching and use the SG that already loaded to VRAM

	; toggle the double-buffering flag

	inner_flags_switch_dbl_buff	; the new SG data will be loaded to opposite VADDR

.skip_dbl_buff_switch:

	inner_flags_dbl_buff_state
	beq .use_main_buff
	
	jmp __attr_transf_XY_IND_dbf

.use_main_buff:

;--- DBL-BUFF ---

	jmp __attr_transf_XY

_push_SG_data:

	ply				; Y - SG bank index

;--- SPD_FLAG_IGNORE_SG ---
	get_SATB_flag SPD_FLAG_IGNORE_SG
	bne .ignore_SG_data
;--- SPD_FLAG_IGNORE_SG ---

	; check if SG data already loaded to VRAM

	cpy <__last_SG_bank
	beq .ignore_SG_data

	call _load_SG_data

	rts

.ignore_SG_data:

	; SG data already loaded or must be ignored

.ifdef	SPD_DEBUG
	call _black_border
.endif

	ldx #1
	cla

	rts

	; transform XY coordinates

__attr_transf_XY:

	cly

;--- SPD_FLAG_ALT_VADDR ---
	get_SATB_flag SPD_FLAG_ALT_VADDR
	beq .__attr_loop_XY

	jmp __attr_transf_XY_IND_alt
;--- SPD_FLAG_ALT_VADDR ---

.__attr_loop_XY:

	iny				;2

	; attr.Y += __spr_pos_y

	lda [<__cx], y			;7 high byte
	tax				;2
	dey				;2
	lda [<__cx], y			;7 low byte

	clc				;2
	adc <__spr_pos_y		;4
	sta [<__cx], y			;7
	txa				;2
	adc <__spr_pos_y + 1		;4
	iny				;2
	sta [<__cx], y			;7 = [46]

	iny				;2 = 50

	; attr.X += __spr_pos_x

	iny
	lda [<__cx], y
	tax
	dey
	lda [<__cx], y

	clc
	adc <__spr_pos_x
	sta [<__cx], y
	txa
	adc <__spr_pos_x + 1
	iny
	sta [<__cx], y

	; move to the next attr line

	tya			;2
	clc			;2
	adc #05			;2
	tay			;2 = 8

	bne .cont		;2 = 10

	inc <__ch
.cont:
	inc <__SATB_pos

	dec <__al			; __al - number of attrs
	bne .__attr_loop_XY

	jmp _push_SG_data

	; transform XY coordinates and modify sprite pattern code
	; this routine is used for alternative VADDR

__attr_transf_XY_IND_alt:

	cly

.__attr_loop_XY_IND_alt:

	iny

	; attr.Y += __spr_pos_y

	lda [<__cx], y
	tax
	dey
	lda [<__cx], y

	clc
	adc <__spr_pos_y
	sta [<__cx], y
	txa
	adc <__spr_pos_y + 1
	iny
	sta [<__cx], y

	iny

	; attr.X += __spr_pos_x

	iny
	lda [<__cx], y
	tax
	dey
	lda [<__cx], y

	clc
	adc <__spr_pos_x
	sta [<__cx], y
	txa
	adc <__spr_pos_x + 1
	iny
	sta [<__cx], y

	iny

	; attr.SG_ind += __spr_alt_SG_offset

	iny
	lda [<__cx], y
	tax
	dey
	lda [<__cx], y

	clc
	adc <__spr_alt_SG_offset
	sta [<__cx], y
	txa
	adc <__spr_alt_SG_offset + 1
	iny
	sta [<__cx], y

	; move to the next attr line

	iny		;2
	iny		;2
	iny		;2 = 6

	bne .cont	;2 = 8

	inc <__ch
.cont:
	inc <__SATB_pos

	dec <__al			; __al - number of attrs
	bne .__attr_loop_XY_IND_alt

	jmp _push_SG_data

	; transform XY coordinates and modify sprite pattern code
	; this routine is used when double-buffering is active

__attr_transf_XY_IND_dbf:

	cly

.__attr_loop_XY_IND_dbf:

	iny

	; attr.Y += __spr_pos_y

	lda [<__cx], y
	tax
	dey
	lda [<__cx], y

	clc
	adc <__spr_pos_y
	sta [<__cx], y
	txa
	adc <__spr_pos_y + 1
	iny
	sta [<__cx], y

	iny

	; attr.X += __spr_pos_x

	iny
	lda [<__cx], y
	tax
	dey
	lda [<__cx], y

	clc
	adc <__spr_pos_x
	sta [<__cx], y
	txa
	adc <__spr_pos_x + 1
	iny
	sta [<__cx], y

	iny

	; attr.SG_ind += __spr_dbf_SG_offset

	iny
	lda [<__cx], y
	tax
	dey
	lda [<__cx], y

	clc
	adc <__spr_dbf_SG_offset
	sta [<__cx], y
	txa
	adc <__spr_dbf_SG_offset + 1
	iny
	sta [<__cx], y

	; move to the next attr line

	iny		;2
	iny		;2
	iny		;2 = 6

	bne .cont	;2 = 8

	inc <__ch
.cont:
	inc <__SATB_pos

	dec <__al			; __al - number of attrs
	bne .__attr_loop_XY_IND_dbf

	jmp _push_SG_data

	.endp

	; load SG data to VRAM

	.proc _load_SG_data

	sty <__last_SG_bank

	; __dx = SG bank index x6 ( .word <data_length>, chrN, bank(chrN) )

	sty <__bh
	stz <__dh
	mul6_bhdh <__dx

	stw <__spr_SG_data_addr, <__si
	lda <__spr_SG_data_bank
	sta <__bl

	call _spd_farptr_add_offset

	jsr map_data			; map SG data array

	; __cx = SG data length

	cly
	lda [<__si], y
	sta <__cl
	iny
	lda [<__si], y
	sta <__ch
	div2_word <__cx

	; __ax = SG data address

	iny
	lda [<__si], y
	sta <__al
	iny
	lda [<__si], y
	sta <__ah

	; __bl = SG data bank

	iny
	lda [<__si], y
	tax

	jsr unmap_data

	stx <__bl

	; __si = __ax

	stw <__ax, <__si

	; __di = VADDR
	
;--- DBL-BUFF ---
	get_SATB_flag SPD_FLAG_DBL_BUFF
	beq .use_main_VADDR		; no double-buffering

	; check which VADDR to use

	inner_flags_dbl_buff_state
	beq .use_main_VADDR		; use the main VADDR

	stw <__spr_VADDR_dbl, <__di	; use opposite VADDR

	bra .cont_load_SG_data

.use_main_VADDR:

;--- DBL-BUFF ---

	stw <__spr_VADDR, <__di

.cont_load_SG_data:

	get_SATB_flag SPD_FLAG_PEND_SG_DATA
	bne .copy_SG_data_params

.ifdef	SPD_DEBUG
	call _load_VRAM_border
.endif
	jsr load_vram

.exit:

.ifdef	SPD_DEBUG
	call _black_border
.endif

	ldx #( 1 | SPD_SG_NEW_DATA )
	cla
	
	rts

.copy_SG_data_params:

	; copy SG data parameters for delayed use on VBLANK

	stw __SG_DATA_SRC_ADDR, <__ax
	stw_zpii <__si, <__ax

	stw __SG_DATA_SRC_BANK, <__ax
	stb_zpii <__bl, <__ax

	stw __SG_DATA_DST_ADDR, <__ax
	stw_zpii <__di, <__ax

	stw __SG_DATA_LEN, <__ax
	stw_zpii <__cx, <__ax

	jmp .exit

	.endp

;// spd_SG_data_params( word __ax / src_addr, word __bx / src_bank, word __cx / dst_addr, word __dx / len )
;
	.proc _spd_SG_data_params.4

	stw <__ax, __SG_DATA_SRC_ADDR
	stw <__bx, __SG_DATA_SRC_BANK
	stw <__cx, __SG_DATA_DST_ADDR
	stw <__dx, __SG_DATA_LEN

	rts

	.endp

;// spd_copy_SG_data_to_VRAM( word __ax / src_addr, byte __bl / src_bank, word __dx / dst_addr, word __cx / len )
;
	.proc _spd_copy_SG_data_to_VRAM.4

	stw <__ax, <__si
	stw <__dx, <__di

.ifdef	SPD_DEBUG
	call _load_VRAM_border

	jsr load_vram

	call _black_border

	rts
.else
	jmp load_vram
.endif
	.endp

;// spd_copy_SG_data_to_VRAM( farptr __bl:__si / addr, byte __bh / index )
;
	.proc _spd_copy_SG_data_to_VRAM.2

	; offset x6 -> sizeof( spd_SPRITE )

	stz <__dh
	mul6_bhdh <__dx

	call _spd_farptr_add_offset

	call _spd_copy_SG_data_to_VRAM.1

	rts

	.endp

;// spd_copy_SG_data_to_VRAM( farptr __bl:__si / addr )
;
	.proc _spd_copy_SG_data_to_VRAM.1

	jsr map_data			; map spd_SPRITE data

	ldy #$05
	lda [<__si], y
	tay				; Y - SG bank index

	jsr unmap_data

	call _load_SG_data

	rts

	.endp

;// spd_SATB_push_simple_sprite( farptr __bl:__si / addr, byte __bh / index, word __ax / x_pos, word __cx / y_pos )
;
	.proc _spd_SATB_push_simple_sprite.4

.ifdef	SPD_DEBUG
	call _push_sprite_border
.endif

	; offset x6 -> sizeof( spd_SPRITE )

	stz <__dh
	mul6_bhdh <__dx

	call _spd_farptr_add_offset

	call _spd_SATB_push_simple_sprite.3

	rts

	.endp

;// spd_SATB_push_simple_sprite( farptr __bl:__si / addr, word __ax / x_pos, word __cx / y_pos )
;
	.proc _spd_SATB_push_simple_sprite.3

.ifdef	SPD_DEBUG
	call _push_sprite_border
.endif

	; XY coordinates correction

	lda #32
	clc
	adc <__ax
	sta <__spr_pos_x
	cla
	adc <__ax + 1
	sta <__spr_pos_x + 1

	lda #64
	clc
	adc <__cx
	sta <__spr_pos_y
	cla
	adc <__cx + 1
	sta <__spr_pos_y + 1

	jsr map_data			; map spd_SPRITE data

	; get meta-sprite address and bank

	cly				; meta-sprite address offset
	stw_zpii_rev __si, <__cx

	ldy #$02			; meta-sprite bank offset
	lda [<__si], y
	sta <__bl			; __bl - meta-sprite bank

	ldy #$04			; 3 - meta-sprite length offset, 4 - meta-sprite SG bank offset
	lda [<__si], y
	tay				; Y - SG bank index

	jsr unmap_data

	stw <__cx, <__si		; __si - matasprite address, __bl - bank

	jsr map_data			; map meta-sprite data

	; __si = meta-sprite address

	; calc SATB address to copy sprite data to
	; _bdsti = __SATB + ( __SATB_pos * 8 )

	lda <__SATB_pos
	sta <__cl
	stz <__ch
	mul8_word <__cx			; __cx - SATB offset in bytes

	add_word_to_word #__SATB, <__cx

	inc <__SATB_pos			; increment SATB position

	phy				; Y - SG bank index

	; transform XY coordinates

	ldy #1

	; transform XY coordinates for main VADDR

	; attr.Y += __spr_pos_y

	lda [<__si], y			;7 high byte
	tax				;2
	dey				;2
	lda [<__si], y			;7 low byte

	clc				;2
	adc <__spr_pos_y		;4
	sta [<__cx], y			;7
	txa				;2
	adc <__spr_pos_y + 1		;4
	iny				;2
	sta [<__cx], y			;7 = [46]

	iny				;2 = 50

	; attr.X += __spr_pos_x

	iny
	lda [<__si], y
	tax
	dey
	lda [<__si], y

	clc
	adc <__spr_pos_x
	sta [<__cx], y
	txa
	adc <__spr_pos_x + 1
	iny
	sta [<__cx], y

;--- SPD_FLAG_ALT_VADDR ---
	get_SATB_flag SPD_FLAG_ALT_VADDR
	bne ._trans_pttrn_code

	; copy pattern code and palette +CGX/CGY

	iny				;2
	lda [<__si], y			;7
	sta [<__cx], y			;7 = (16)

	iny
	lda [<__si], y
	sta [<__cx], y

	iny
	lda [<__si], y
	sta [<__cx], y

	iny
	lda [<__si], y
	sta [<__cx], y

	bra ._push_SG_data

._trans_pttrn_code:

;--- SPD_FLAG_ALT_VADDR ---

	; transform pattern code for alternative VADDR

	iny

	; attr.SG_ind += __spr_alt_SG_offset

	iny
	lda [<__si], y
	tax
	dey
	lda [<__si], y

	clc
	adc <__spr_alt_SG_offset
	sta [<__cx], y
	txa
	adc <__spr_alt_SG_offset + 1
	iny
	sta [<__cx], y

	; copy palette and CGX/CGY

	iny
	lda [<__si], y
	sta [<__cx], y

	iny
	lda [<__si], y
	sta [<__cx], y

._push_SG_data:

	jsr unmap_data

	ply				; Y - SG bank index

;--- SPD_FLAG_IGNORE_SG ---
	get_SATB_flag SPD_FLAG_IGNORE_SG
	bne .ignore_SG_data
;--- SPD_FLAG_IGNORE_SG ---

	; check if SG data already loaded to VRAM

	cpy <__last_SG_bank
	bne .load_SG_data

.ignore_SG_data:

	; SG data already loaded or must be ignored

.ifdef	SPD_DEBUG
	call _black_border
.endif

	ldx #1
	cla

	rts

.load_SG_data:

	sty <__last_SG_bank

	; __dx = SG bank index x6 ( .word <data_length>, chrN, bank(chrN) )

	sty <__bh
	stz <__dh
	mul6_bhdh <__dx

	stw <__spr_SG_data_addr, <__si
	lda <__spr_SG_data_bank
	sta <__bl

	call _spd_farptr_add_offset

	jsr map_data			; map SG data array

	; __cx = SG data length

	cly
	lda [<__si], y
	sta <__cl
	iny
	lda [<__si], y
	sta <__ch
	div2_word <__cx

	; __ax = SG data address

	iny
	lda [<__si], y
	sta <__al
	iny
	lda [<__si], y
	sta <__ah

	; __bl = SG data bank

	iny
	lda [<__si], y
	tax

	jsr unmap_data

	stx <__bl

	; __si = __ax

	stw <__ax, <__si

	; __di = VADDR
	
	stw <__spr_VADDR, <__di

	get_SATB_flag SPD_FLAG_PEND_SG_DATA
	beq .cont_SG_data_load

	; copy SG data parameters for delayed use on VBLANK

	stw __SG_DATA_SRC_ADDR, <__ax
	stw_zpii <__si, <__ax

	stw __SG_DATA_SRC_BANK, <__ax
	stb_zpii <__bl, <__ax

	stw __SG_DATA_DST_ADDR, <__ax
	stw_zpii <__di, <__ax

	stw __SG_DATA_LEN, <__ax
	stw_zpii <__cx, <__ax

	bra .exit

.cont_SG_data_load:

.ifdef	SPD_DEBUG
	call _load_VRAM_border
.endif
	jsr load_vram

.exit:

.ifdef	SPD_DEBUG
	call _black_border
.endif

	ldx #( 1 | SPD_SG_NEW_DATA )
	cla
	
	rts

	.endp

; for debugging purposes
.ifdef SPD_DEBUG
	.proc _black_border

	clx
	cly

	call _set_border_color

	rts

	.endp

	.proc _push_sprite_border

	ldx #$01
	ldy #$ff

	call _set_border_color

	rts

	.endp

	.proc _load_VRAM_border

	clx
	ldy #$ff

	call _set_border_color

	rts

	.endp

	.proc _set_border_color

	stw #$0100, $0402

	sty $0404
	stx $0405

	rts

	.endp

.endif	;SPD_DEBUG

	.endprocgroup

#endasm