import PaginationOptions from "./paginationOptions";
import {useState} from "react";

export const usePagination = ({startPage, minPage}: PaginationOptions = {startPage: 1, minPage: 1}): [
    number,
    () => (number),
    () => (number),
    () => (number),
] => {
    const lowerPage = 1;
    const [page, setPage] = useState(startPage ?? 1);
    const reset = () => {
        setPage(lowerPage);
        return lowerPage;
    }

    const toNextPage = () => {
        setPage(page + 1)
        return page + 1
    }

    const toPrevPage = () => {
        const prevPage = page - 1;
        if (prevPage < lowerPage) {
            throw new RangeError(`Attempted to go under ${lowerPage} page`)
        }
        setPage(prevPage);
        return prevPage;
    }

    return [page, toNextPage, toPrevPage, reset];
}