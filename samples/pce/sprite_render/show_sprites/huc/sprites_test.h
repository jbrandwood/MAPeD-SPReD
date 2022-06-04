//#######################################################
//
// Generated by SPReD-PCE Copyright 2017-2022 0x8BitDev
//
//#######################################################

#incasm( "sprites_test.asm" )

#define SPRITES_TEST_SPR_VADDR	8192
#define SPRITES_TEST_PALETTE_SLOT	16


#ifndef	DEF_TYPE_SPD_SPRITE
#define	DEF_TYPE_SPD_SPRITE
typedef struct
{
	const unsigned short*	attrs;
	unsigned char		bank;
	unsigned short		size;
	unsigned char		SG_ind;
} spd_SPRITE;
#endif	//DEF_TYPE_SPD_SPRITE


extern unsigned short*	sprites_test_SG_arr;
const unsigned char	sprites_test_SG_cnt	= 1;	// graphics banks count
const unsigned short	sprites_test_palette_size	= 6;	// active palettes

extern unsigned short*	sprites_test_palette;
const unsigned short	sprites_test_frames_cnt	= 10;
extern spd_SPRITE	sprites_test_frames_data[];

const unsigned char	SPR_JCH_RIGHT_32X64	= 0;
const unsigned char	SPR_DR_MSL_UP_16X64_0	= 1;
const unsigned char	SPR_DR_MSL_UP_16X64_1_REF	= 2;
const unsigned char	SPR_JCH_MIN_16X32_0	= 3;
const unsigned char	SPR_JCH_MIN_16X32_1_REF	= 4;
const unsigned char	SPR_RPL_FLY_RIGHT_32X32	= 5;
const unsigned char	SPR_BRSTICK_RIGHT_32X16	= 6;
const unsigned char	SPR_DR_MSL_UP	= 7;
const unsigned char	SPR_GIGAN_IDLE_LEFT	= 8;
const unsigned char	SPR_TONY_IDLE_RIGHT	= 9;

extern spd_SPRITE*	jch_RIGHT_32x64_frame;
extern spd_SPRITE*	dr_msl_UP_16x64_0_frame;
extern spd_SPRITE*	dr_msl_UP_16x64_1_ref_frame;
extern spd_SPRITE*	jch_min_16x32_0_frame;
extern spd_SPRITE*	jch_min_16x32_1_ref_frame;
extern spd_SPRITE*	rpl_fly_RIGHT_32x32_frame;
extern spd_SPRITE*	brstick_RIGHT_32x16_frame;
extern spd_SPRITE*	dr_msl_UP_frame;
extern spd_SPRITE*	gigan_idle_LEFT_frame;
extern spd_SPRITE*	tony_idle_RIGHT_frame;

