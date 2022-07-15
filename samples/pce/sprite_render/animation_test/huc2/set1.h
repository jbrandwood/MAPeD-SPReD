//#######################################################
//
// Generated by SPReD-PCE Copyright 2017-2022 0x8BitDev
//
//#######################################################

#incasm( "data/set1.asm" )

#define SET1_SPR_VADDR	0x2000

#ifndef	DEF_TYPE_SPD_SPRITE
#define	DEF_TYPE_SPD_SPRITE
typedef struct
{
	const unsigned short	size;
	const unsigned char	SG_ind;
	const unsigned short	attrs[];
} spd_SPRITE;
#endif	//DEF_TYPE_SPD_SPRITE


#define	SET1_SG_CNT	10	// graphics banks count
extern unsigned short*	set1_SG_arr;

#define	SET1_PALETTE_SIZE	1	// active palettes
#define SET1_PALETTE_SLOT	16

extern unsigned short*	set1_palette_slot0;
extern unsigned short*	set1_palette;

#define	SET1_FRAMES_CNT	12
extern unsigned char*	set1_frames_data;

#define	SPR_MPWR_IDLE01_RIGHT	0
#define	SPR_MPWR_WALK01_RIGHT	1
#define	SPR_MPWR_WALK02_RIGHT	2
#define	SPR_MPWR_WALK03_RIGHT	3
#define	SPR_MPWR_WALK04_RIGHT	4
#define	SPR_MPWR_WALK05_RIGHT	5
#define	SPR_MPWR_WALK06_RIGHT	6
#define	SPR_MPWR_KICK01_RIGHT	7
#define	SPR_MPWR_KICK02_RIGHT	8
#define	SPR_MPWR_KICK03_RIGHT	9
#define	SPR_MPWR_KICK04_REF02_RIGHT	10
#define	SPR_MPWR_KICK05_REF01_RIGHT	11

extern spd_SPRITE*	mpwr_idle01_RIGHT;
extern spd_SPRITE*	mpwr_walk01_RIGHT;
extern spd_SPRITE*	mpwr_walk02_RIGHT;
extern spd_SPRITE*	mpwr_walk03_RIGHT;
extern spd_SPRITE*	mpwr_walk04_RIGHT;
extern spd_SPRITE*	mpwr_walk05_RIGHT;
extern spd_SPRITE*	mpwr_walk06_RIGHT;
extern spd_SPRITE*	mpwr_kick01_RIGHT;
extern spd_SPRITE*	mpwr_kick02_RIGHT;
extern spd_SPRITE*	mpwr_kick03_RIGHT;
extern spd_SPRITE*	mpwr_kick04_ref02_RIGHT;
extern spd_SPRITE*	mpwr_kick05_ref01_RIGHT;

