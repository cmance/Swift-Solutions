import { validateBuildBookmarkListCardProps } from '../../PopNGo/wwwroot/js/ui/buildBookmarkListCard.js';
import { validateBuildEventCardProps } from '../../PopNGo/wwwroot/js/ui/buildEventCard.js';
import { validateBuildEventDetailsModalProps } from '../../PopNGo/wwwroot/js/ui/buildEventDetailsModal.js';
import { validateNewBuildBookmarkListCardProps } from '../../PopNGo/wwwroot/js/ui/buildNewBookmarkListCard.js';


describe('validateBuildEventCardProps', () => {
    test('null props should return false', () => {
        expect(validateBuildEventCardProps(null)).toBe(false);
    });

    test('undefined props should return false', () => {
        expect(validateBuildEventCardProps(undefined)).toBe(false);
    });

    test('non-object props should return false', () => {
        expect(validateBuildEventCardProps(1)).toBe(false);
    });

    test('correct props should return true', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });

    test('img not a string should return false', () => {
        const props = {
            img: 1,
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('img is null should return true', () => {
        const props = {
            img: null,
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });

    test('img is undefined should return true', () => {
        const props = {
            img: undefined,
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });

    test('title not a string should return false', () => {
        const props = {
            img: "img",
            title: 1,
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('date not a date should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: 1,
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('city not a string should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: 1,
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('state not a string should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: 1,
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('tags not an array should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: 1,
            favorited: false,
            onPressFavorite: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('bookmarkListNames not a list should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: 'bookmarkListNames',
            onPressBookmarkList: () => { },
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('onPressBookmarkList not a function should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: 'WRONG',
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('onPressBookmarkList is null should return true', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: null,
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });

    test('onPressBookmarkList is undefined should return true', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: undefined,
            onPressEvent: () => { }
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });

    test('onPressEvent not a function should return false', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: () => { },
            onPressEvent: 1
        }

        expect(validateBuildEventCardProps(props)).toBe(false);
    });

    test('onPressEvent is null should return true', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: () => { },
            onPressEvent: null
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });

    test('onPressEvent is undefined should return true', () => {
        const props = {
            img: "img",
            title: "title",
            date: new Date(),
            city: "city",
            state: "state",
            tags: [],
            bookmarkListNames: [],
            onPressBookmarkList: () => { },
            onPressEvent: undefined
        }

        expect(validateBuildEventCardProps(props)).toBe(true);
    });
});

describe('validateBuildEventDetailsModalProps', () => {
    test('null props should return false', () => {
        expect(validateBuildEventDetailsModalProps(null)).toBe(false);
    });

    test('undefined props should return false', () => {
        expect(validateBuildEventDetailsModalProps(undefined)).toBe(false);
    });

    test('non-object props should return false', () => {
        expect(validateBuildEventDetailsModalProps(1)).toBe(false);
    });

    test('correct props should return true', () => {
        const props = {
            img: "img",
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(true);
    });

    test('img not a string should return false', () => {
        const props = {
            img: 1,
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('img is null should return true', () => {
        const props = {
            img: null,
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(true);
    });

    test('img is undefined should return true', () => {
        const props = {
            img: undefined,
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(true);
    });

    test('title not a string should return false', () => {
        const props = {
            img: "img",
            title: 1,
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('description not a string should return false', () => {
        const props = {
            img: "img",
            title: "title",
            description: 1,
            date: new Date(),
            fullAddress: "fullAddress",
            tags: [],
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('date not a date should return false', () => {
        const props = {
            img: "img",
            title: "title",
            description: "description",
            date: 1,
            fullAddress: "fullAddress",
            tags: [],
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('fullAddress not a string should return false', () => {
        const props = {
            img: "img",
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: 1,
            tags: [],
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });

    test('tags not an array should return false', () => {
        const props = {
            img: "img",
            title: "title",
            description: "description",
            date: new Date(),
            fullAddress: "fullAddress",
            tags: 1,
        }

        expect(validateBuildEventDetailsModalProps(props)).toBe(false);
    });
});

describe('validateBuildBookmarkListCardProps', () => {
    test('null props should return false', () => {
        expect(validateBuildBookmarkListCardProps(null)).toBe(false);
    });

    test('undefined props should return false', () => {
        expect(validateBuildBookmarkListCardProps(undefined)).toBe(false);
    });

    test('non-object props should return false', () => {
        expect(validateBuildBookmarkListCardProps(1)).toBe(false);
    });

    test('correct props should return true', () => {
        const props = {
            bookmarkListName: "bookmarkListName",
            eventQuantity: 1,
            onClick: () => { }
        }

        expect(validateBuildBookmarkListCardProps(props)).toBe(true);
    });

    test('bookmarkListName not a string should return false', () => {
        const props = {
            bookmarkListName: 1,
            eventQuantity: 1,
            onClick: () => { }
        }

        expect(validateBuildBookmarkListCardProps(props)).toBe(false);
    });

    test('eventQuantity not a number should return false', () => {
        const props = {
            bookmarkListName: "bookmarkListName",
            eventQuantity: "1",
            onClick: () => { }
        }

        expect(validateBuildBookmarkListCardProps(props)).toBe(false);
    });

    test('onClick not a function should return false', () => {
        const props = {
            bookmarkListName: "bookmarkListName",
            eventQuantity: 1,
            onClick: 1
        }

        expect(validateBuildBookmarkListCardProps(props)).toBe(false);
    });

    test('onClick is null should return true', () => {
        const props = {
            bookmarkListName: "bookmarkListName",
            eventQuantity: 1,
            onClick: null
        }

        expect(validateBuildBookmarkListCardProps(props)).toBe(true);
    });

    test('onClick is undefined should return true', () => {
        const props = {
            bookmarkListName: "bookmarkListName",
            eventQuantity: 1,
            onClick: undefined
        }

        expect(validateBuildBookmarkListCardProps(props)).toBe(true);
    });

    test('image not a string should return false', () => {
        const props = {
            bookmarkListName: "bookmarkListName",
            eventQuantity: 1,
            image: 1,
            onClick: () => { }
        }

        expect(validateBuildBookmarkListCardProps(props)).toBe(false);
    });
});

describe("validateNewBuildBookmarkListCardProps", () => {
    test('null props should return true', () => {
        expect(validateNewBuildBookmarkListCardProps(null)).toBe(true);
    });

    test('undefined props should return true', () => {
        expect(validateNewBuildBookmarkListCardProps(undefined)).toBe(true);
    });

    test('correct props should return true', () => {
        const props = {
            onClickCreateBookmarkList: () => { }
        }

        expect(validateNewBuildBookmarkListCardProps(props)).toBe(true);
    });

    test('onClickCreateBookmarkList not a function should return false', () => {
        const props = {
            onClickCreateBookmarkList: 1
        }

        expect(validateNewBuildBookmarkListCardProps(props)).toBe(false);
    });

    test('onClickCreateBookmarkList is null should return true', () => {
        const props = {
            onClickCreateBookmarkList: null
        }

        expect(validateNewBuildBookmarkListCardProps(props)).toBe(true);
    });

    test('onClickCreateBookmarkList is undefined should return true', () => {
        const props = {
            onClickCreateBookmarkList: undefined
        }

        expect(validateNewBuildBookmarkListCardProps(props)).toBe(true);
    });
});
