//#######################################################
//
// Generated by SPReD-PCE Copyright 2017-2022 0x8BitDev
//
//#######################################################

#incasm( "data/chin.asm" )

#define CHIN_SPR_VADDR	0x2000

#ifndef	DEF_TYPE_SPD_SPRITE
#define	DEF_TYPE_SPD_SPRITE
typedef struct
{
	const unsigned char	SG_ind;
	const unsigned short	size;
	const unsigned short	attrs[];
} spd_SPRITE;
#endif	//DEF_TYPE_SPD_SPRITE


#define	CHIN_SG_CNT	5	// graphics banks count
extern unsigned short*	chin_SG_arr;

#define	CHIN_PALETTE_SIZE	1	// active palettes
#define CHIN_PALETTE_SLOT	16

extern unsigned short*	chin_palette_slot0;
extern unsigned short*	chin_palette;

#define	CHIN_FRAMES_CNT	10
extern unsigned char*	chin_frames_data;

#define	SPR_CHIN_IDLE01_RIGHT	0
#define	SPR_CHIN_WALK01_RIGHT	1
#define	SPR_CHIN_WALK02_RIGHT	2
#define	SPR_CHIN_WALK03_RIGHT	3
#define	SPR_CHIN_WALK04_RIGHT	4
#define	SPR_CHIN_IDLE01_LEFT	5
#define	SPR_CHIN_WALK01_LEFT	6
#define	SPR_CHIN_WALK02_LEFT	7
#define	SPR_CHIN_WALK03_LEFT	8
#define	SPR_CHIN_WALK04_LEFT	9

extern spd_SPRITE*	chin_idle01_RIGHT;
extern spd_SPRITE*	chin_walk01_RIGHT;
extern spd_SPRITE*	chin_walk02_RIGHT;
extern spd_SPRITE*	chin_walk03_RIGHT;
extern spd_SPRITE*	chin_walk04_RIGHT;
extern spd_SPRITE*	chin_idle01_LEFT;
extern spd_SPRITE*	chin_walk01_LEFT;
extern spd_SPRITE*	chin_walk02_LEFT;
extern spd_SPRITE*	chin_walk03_LEFT;
extern spd_SPRITE*	chin_walk04_LEFT;

