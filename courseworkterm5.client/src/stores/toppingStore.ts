import type { PizzaDiameterCm } from "@/enums/pizzaDiameter";
import { ref } from "vue";
import { defineStore } from "pinia";

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
export interface Variant {
    diameterCm: PizzaDiameterCm,
    weightGram: number,
    priceRub: number
}

export interface ToppingVariant extends Variant {
}

export interface Topping {
    toppingId: number,
    name: string,
    imageUrl: URL,
    variants: ToppingVariant[]
}

export const useToppingStore = defineStore('toppingStore', () => {
    const toppings = ref<Topping[]>([]);
    const isLoading = ref(false);
    const isLoaded = ref(false);

    async function fetchToppings() {
        isLoading.value = true;
        const response = await fetch('api/toppings');

        if (response.ok) {
            toppings.value = await response.json();
            isLoaded.value = true;
        }
        isLoading.value = false;
    }

    return { toppings, fetchToppings, isLoaded, isLoading };
})