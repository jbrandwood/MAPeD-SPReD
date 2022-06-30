//#######################################################
//
// Generated by SPReD-PCE Copyright 2017-2022 0x8BitDev
//
//#######################################################

#incasm( "data/set2.asm" )

#define SET2_SPR_VADDR	0x3000

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


#define	SET2_SG_CNT	5	// graphics banks count
extern unsigned short*	set2_SG_arr;

#define	SET2_PALETTE_SIZE	1	// active palettes
#define SET2_PALETTE_SLOT	17

extern unsigned short*	set2_palette_slot0;
extern unsigned short*	set2_palette;

#define	SET2_FRAMES_CNT	6
extern spd_SPRITE	set2_frames_data[];

#define	SPR_MARTHA_PUNCH01	0
#define	SPR_MARTHA_PUNCH02	1
#define	SPR_MARTHA_PUNCH03	2
#define	SPR_MARTHA_PUNCH04	3
#define	SPR_MARTHA_PUNCH05	4
#define	SPR_MARTHA_PUNCH06_REF04	5

extern spd_SPRITE*	martha_punch01_frame;
extern spd_SPRITE*	martha_punch02_frame;
extern spd_SPRITE*	martha_punch03_frame;
extern spd_SPRITE*	martha_punch04_frame;
extern spd_SPRITE*	martha_punch05_frame;
extern spd_SPRITE*	martha_punch06_ref04_frame;

