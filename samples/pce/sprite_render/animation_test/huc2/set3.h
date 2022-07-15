//#######################################################
//
// Generated by SPReD-PCE Copyright 2017-2022 0x8BitDev
//
//#######################################################

#incasm( "data/set3.asm" )

#define SET3_SPR_VADDR	0x4000

#ifndef	DEF_TYPE_SPD_SPRITE
#define	DEF_TYPE_SPD_SPRITE
typedef struct
{
	const unsigned char	SG_ind;
	const unsigned short	size;
	const unsigned short	attrs[];
} spd_SPRITE;
#endif	//DEF_TYPE_SPD_SPRITE


#define	SET3_SG_CNT	10	// graphics banks count
extern unsigned short*	set3_SG_arr;

#define	SET3_PALETTE_SIZE	1	// active palettes
#define SET3_PALETTE_SLOT	18

extern unsigned short*	set3_palette_slot0;
extern unsigned short*	set3_palette;

#define	SET3_FRAMES_CNT	12
extern unsigned char*	set3_frames_data;

#define	SPR_MPWR_IDLE01_LEFT	0
#define	SPR_MPWR_WALK01_LEFT	1
#define	SPR_MPWR_WALK02_LEFT	2
#define	SPR_MPWR_WALK03_LEFT	3
#define	SPR_MPWR_WALK04_LEFT	4
#define	SPR_MPWR_WALK05_LEFT	5
#define	SPR_MPWR_WALK06_LEFT	6
#define	SPR_MPWR_KICK01_LEFT	7
#define	SPR_MPWR_KICK02_LEFT	8
#define	SPR_MPWR_KICK03_LEFT	9
#define	SPR_MPWR_KICK04_REF02_LEFT	10
#define	SPR_MPWR_KICK05_REF01_LEFT	11

extern spd_SPRITE*	mpwr_idle01_LEFT;
extern spd_SPRITE*	mpwr_walk01_LEFT;
extern spd_SPRITE*	mpwr_walk02_LEFT;
extern spd_SPRITE*	mpwr_walk03_LEFT;
extern spd_SPRITE*	mpwr_walk04_LEFT;
extern spd_SPRITE*	mpwr_walk05_LEFT;
extern spd_SPRITE*	mpwr_walk06_LEFT;
extern spd_SPRITE*	mpwr_kick01_LEFT;
extern spd_SPRITE*	mpwr_kick02_LEFT;
extern spd_SPRITE*	mpwr_kick03_LEFT;
extern spd_SPRITE*	mpwr_kick04_ref02_LEFT;
extern spd_SPRITE*	mpwr_kick05_ref01_LEFT;

