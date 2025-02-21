import type PaymentType from "@/types/PaymentType";
import type Customer from "./Customer";
import type { Purchase } from "@/stores/shoppingCartStore";

export interface Order {
    customer: Customer,
    createdAt: Date,
    closedAt?: Date,
    useShipping: boolean,
    paymentType: PaymentType,
    totalPriceRub: number,
    products: Purchase[],
}

export type OrderModel = Omit<Order, 'createdAt' | 'closedAt'>;