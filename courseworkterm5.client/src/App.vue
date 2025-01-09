<script setup lang="ts">
import { RouterView } from 'vue-router'
import SiteHeader from '@/components/SiteHeader.vue'
import { usePersistShoppingCartState } from './stores/shoppingCartStore';

usePersistShoppingCartState();
</script>

<template>
  <SiteHeader />

  <RouterView />

  <footer>

  </footer>
</template>
