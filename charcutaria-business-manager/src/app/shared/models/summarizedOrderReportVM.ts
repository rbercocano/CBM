import { OrderItemReport } from './orderItemReport';
import { SummarizedOrderReport } from './summarizedOrderReport';

export interface SummarizedOrderReportVM extends SummarizedOrderReport {
    isExpanded?: boolean;
    orders?: OrderItemReport[];
}