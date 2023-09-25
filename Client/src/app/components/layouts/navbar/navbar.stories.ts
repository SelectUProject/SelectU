import type { Meta, StoryObj } from '@storybook/angular';

import { action } from '@storybook/addon-actions';

import NavbarComponent from './navbar.component';
const meta: Meta<NavbarComponent> = {
  title: 'Navbar',
  component: NavbarComponent,
  tags: ['autodocs'],
  render: (args: NavbarComponent) => ({
    props: {
      ...args,
    },
  }),
};

export default meta;
type Story = StoryObj<NavbarComponent>;

export const Default: Story = {
  args: {
    title: 'Darcy',
  },
};
