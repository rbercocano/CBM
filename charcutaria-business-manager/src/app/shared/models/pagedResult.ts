import { PaginationInfo } from './paginationInfo';

export interface PagedResult<T>  extends PaginationInfo{
    data: T[];
}