import type { ToppingPreview } from '@/components/ToppingCardCheckbox.vue';
import type PizzaDiameterCm from '@/types/PizzaDiameterCm';
import { defineStore } from 'pinia'
import { computed, ref, onMounted } from 'vue';

export const CART_STORE_ID = 'tastyPizzaCart';

export function usePersistShoppingCartState() {
    onMounted(() => {
        window.addEventListener('beforeunload', () => {
            persistShoppingCartState();
        })
    });
}

function persistShoppingCartState() {
    const cartStore = useShoppingCartStore();
    localStorage.setItem(CART_STORE_ID, JSON.stringify(cartStore.items));
}

export function resetShoppingCartState() {
    const cartStore = useShoppingCartStore();
    localStorage.removeItem(CART_STORE_ID);
    cartStore.$reset();
}

export interface CartItem {
    productId: number,
    name: string,
    description?: string,
    imageUrl: URL,
    priceRub: number,
    diameterCm?: PizzaDiameterCm,
    toppings?: ToppingPreview[]
}

export interface CartItemPizza {
    productId: number,
    name: string,
    description: string,
    imageUrl: URL,
    priceRub: number,
    diameterCm: PizzaDiameterCm,
    toppings?: ToppingPreview[]
}

export interface Purchase {
    cartItem: CartItem,
    quantity: number
}

export const useShoppingCartStore = defineStore(CART_STORE_ID, () => {
    const items = ref<Purchase[]>(
        JSON.parse(localStorage.getItem(CART_STORE_ID) as string)
        ?? []
    );

    const isEmpty = computed(() => {
        return items.value.length === 0;
    })

    const totalQuantity = computed(() => {
        if (isEmpty.value) {
            return 0;
        }

        return items.value.reduce((quantityAccum, currentItem) => {
            return quantityAccum + currentItem.quantity
        }, 0);
    })

    const totalPriceRub = computed(() => {
        if (isEmpty.value) {
            return 0;
        }

        return items.value.reduce((priceRubAccum, currentItem) => {
            return priceRubAccum +
                currentItem.cartItem.priceRub * currentItem.quantity
        }, 0);
    })

    // !!!!!!!!!!!!!!!!!!!!!! cartItem вместо productId 
    function isItemInCart(productId: number): boolean {
        return items.value
            .find(p => p.cartItem.productId === productId) !== undefined
    }

    // !!!!!!!!!!!!!!!!!!!!!! cartItem вместо productId
    function getItemQuantity(productId: number) {
        return items.value
            .find(p => p.cartItem.productId === productId)?.quantity;
    }

    function addPizza(pizza: CartItemPizza, quantity: number) {
        items.value.push({
            cartItem: pizza,
            quantity: quantity
        });
    }

    function addOneProductUnit(product: CartItem) {
        const purchase = items.value
            .find(p => p.cartItem.productId === product.productId);

        if (purchase) {
            if (purchase.quantity < 9) {
                purchase.quantity++;
            }
        }
        else {
            items.value.push({
                cartItem: product,
                quantity: 1
            });
        }
    }

    function removeOneProductUnit(product: CartItem) {
        const purchase = items.value
            .find(p => p.cartItem.productId === product.productId);

        if (purchase) {
            if (purchase.quantity > 1) {
                purchase.quantity--;
                return false;
            }
            else {
                removePurchase(purchase);
                return true;
            }
        }
    }

    function removeItem(productId: number) {
        const purchase = items.value
            .find(p => p.cartItem.productId === productId);

        if (purchase) {
            removePurchase(purchase);
        }
    }

    function removePurchase(purchaseToRemove: Purchase) {
        const purchaseIndex = items.value.indexOf(purchaseToRemove);
        items.value.splice(purchaseIndex, 1);
    }

    function $reset() {
        items.value.length = 0;
      }

    return {
        items,
        totalQuantity,
        totalPriceRub,
        isEmpty,
        addOneProductUnit,
        addPizza,
        removeOneProductUnit,
        removePurchase,
        isItemInCart,
        getItemQuantity,
        removeItem,
        $reset
    }
});

/*
export const useCartStore2 = defineStore('cart', {
    state: () => ({
        serviceIdMapQuantity: JSON.parse(localStorage.getItem(CART_STORE_ID) ?? ) ?? {}
    }),
    getters: {
        totalQuantity(state) {
            return Object
                .values(state.serviceIdMapQuantity)
                .reduce(
                    (quantityAccumulator, currentValue) => quantityAccumulator + currentValue
                    , 0);
        },
        totalCost(state) {
            const serviceStore = useServiceStore();
            return Object
                .keys(state.serviceIdMapQuantity)
                .reduce(
                    (priceAccumulator, currentKey) => priceAccumulator + serviceStore.findServiceById(Number(currentKey)).price *
                        state.serviceIdMapQuantity[currentKey]
                    , 0
                )
        },
        isEmpty(state) {
            return Object.keys(state.serviceIdMapQuantity).length === 0;
        },
        formattedCart(state) {
            const serviceStore = useServiceStore();

            return Object
                .entries(state.serviceIdMapQuantity)
                .map(idQuantityPair => {
                    const id = idQuantityPair[0];
                    const quantity = idQuantityPair[1];
                    const service = serviceStore.findServiceById(Number(id));

                    return {
                        id: Number(id),
                        name: service.name,
                        description: service.description,
                        totalPrice: quantity * service.price,
                        quantity: quantity,
                        imgUrl: service.imgUrl
                    }
                });
        }
    },
    actions: {
        addService(serviceId) {
            if (this.serviceIdMapQuantity[serviceId] === 9) {
                return;
            }

            if (this.serviceIdMapQuantity[serviceId]) {
                this.serviceIdMapQuantity[serviceId]++;
            } else {
                this.serviceIdMapQuantity[serviceId] = 1;
            }
        },
        removeService(serviceId) {
            if (!this.serviceIdMapQuantity[serviceId]) {
                return;
            }

            this.serviceIdMapQuantity[serviceId]--;

            if (this.serviceIdMapQuantity[serviceId] === 0) {
                delete this.serviceIdMapQuantity[serviceId];
            }
        }

    }
});
*/